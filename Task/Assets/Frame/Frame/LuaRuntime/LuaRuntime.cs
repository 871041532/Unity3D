// -----------------------------------------------------------------
// File:    LuaRuntime.cs
// Author:  mouguangyi
// Date:    2016.07.22
// Description:
//      
// -----------------------------------------------------------------
using AOT;
using GameBox.Framework;
using GameBox.Service.AssetManager;
using KeraLuaLite;
using NLuaLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBox.Service.LuaRuntime
{
    class LuaRuntime : ILuaRuntime
    {
        public string Id
        {
            get {
                return "com.giant.service.luaruntime";
            }
        }

        public void Run(IServiceRunner runner)
        {
            this.luaState = LuaLib.LuaLNewState();
            LuaLib.LuaLOpenLibs(this.luaState);

            // We need to keep this in a managed reference so the delegate doesn't get garbage collected
            this.panicCallback = new LuaNativeFunction(_PanicCallback);
            LuaLib.LuaAtPanic(this.luaState, this.panicCallback);

            // Inject print module with our own function
            // - mouguangyi
            this.printCallback = new LuaNativeFunction(_PrintCallback);
            LuaLib.LuaPushStdCallCFunction(luaState, this.printCallback);
            LuaLib.LuaSetGlobal(this.luaState, "print");

            // Inject dofile module with our own function
            // - mouguangyi
            this.dofileCallback = new LuaNativeFunction(_DoFileCallback);
            LuaLib.LuaPushStdCallCFunction(luaState, this.dofileCallback);
            LuaLib.LuaSetGlobal(this.luaState, "dofile");

            // Inject searcher module with our own function
            // - mouguangyi
            this.searcherCallback = new LuaNativeFunction(_SearcherCallback);
            LuaLib.LuaGetGlobal(this.luaState, "package");
            LuaLib.LuaGetField(this.luaState, -1, "loaders");
            LuaLib.LuaPushStdCallCFunction(this.luaState, this.searcherCallback);
            for (var i = (LuaLib.LuaObjLen(this.luaState, -2) + 1); i >= 2; --i) {
                LuaLib.LuaRawGetI(this.luaState, -2, i - 1);
                LuaLib.LuaRawSetI(this.luaState, -3, i);
            }
            LuaLib.LuaRawSetI(this.luaState, -2, 2);
            LuaLib.LuaPop(this.luaState, 2);

            // Create luaruntime metatable
            // - mouguangyi
            this.collectCallback = new LuaNativeFunction(LuaExecuter._CollectObject);
            LuaLib.LuaLNewMetatable(this.luaState, "luaruntime");
            LuaLib.LuaPushString(this.luaState, "__gc");
            LuaLib.LuaPushStdCallCFunction(this.luaState, this.collectCallback);
            LuaLib.LuaSetTable(this.luaState, -3);

            new ServiceTask("com.giant.service.assetmanager").Start().Continue(task =>
            {
                runner.Ready(_Terminate);
                return null;
            });
        }

        public void Pulse(float delta)
        { }

        public object[] DoString(byte[] chunk)
        {
            try {
                int oldTop = LuaLib.LuaGetTop(this.luaState);
                if (LuaLib.LuaLLoadBuffer(this.luaState, chunk, "chunkName") == 0) {
                    if (0 == LuaLib.LuaPCall(this.luaState, 0, -1, 0)) {
                        return _PopValues(this.luaState, oldTop);
                    }
                }

                _ThrowExceptionFromError(this.luaState, oldTop);
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }

            return null;
        }

        public object[] DoFile(string fileName)
        {
            // Use asset manager to replace original load file process.
            // - mouguangyi
            try {
                using (var asset = ServiceCenter.GetService<IAssetManager>().Load(fileName + ".txt", AssetType.BYTES)) {
                    var chunk = asset.Cast<byte[]>();
                    if (null == chunk) {
                        throw new Exception("Can't load lua script: " + fileName);
                    } else {
                        int oldTop = LuaLib.LuaGetTop(this.luaState);
                        if (0 == LuaLib.LuaLLoadBuffer(this.luaState, chunk, fileName)) {
                            if (0 == LuaLib.LuaPCall(this.luaState, 0, -1, 0)) {
                                return _PopValues(this.luaState, oldTop);
                            }
                        }

                        _ThrowExceptionFromError(this.luaState, oldTop);
                    }
                }
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }

            return null;
        }

        public object[] Call(object function, params object[] args)
        {
            int oldTop = LuaLib.LuaGetTop(this.luaState);

            if (function is string) {
                var fullPath = function as string;
                var path = fullPath.Split('.');
                LuaLib.LuaGetGlobal(this.luaState, path[0]);
                function = LuaExecuter._ParseLuaValue(luaState, -1);
                if (path.Length > 1) {
                    var remainingPath = new string[path.Length - 1];
                    Array.Copy(path, 1, remainingPath, 0, path.Length - 1);
                    function = _GetObject(remainingPath);
                }
            }

            LuaExecuter._PushObject(this.luaState, function);
            if (null != args) {
                for (int i = 0; i < args.Length; i++) {
                    LuaExecuter._PushObject(this.luaState, args[i]);
                }
            }

            try {
                int error = LuaLib.LuaPCall(this.luaState, args.Length, -1, 0);
                if (error != 0) {
                    _ThrowExceptionFromError(this.luaState, oldTop);
                }
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }

            return _PopValues(this.luaState, oldTop);
        }

        // Export native function to lua.
        // - mouguangyi 2016.07.28
        public void RegLuaBridgeFunction(LuaBridgeFunction function)
        {
            var executer = new LuaExecuter(this, function);
            LuaLib.LuaPushNumber(luaState, executer.Index);
            LuaLib.LuaPushCClosure(luaState, LuaExecuter._Execute, 1);
            LuaLib.LuaSetGlobal(luaState, executer.Name);
        }

        // Pass LuaState to 3rd C#/C function to attach C lib into current LuaState sandbox
        // - mouguangyi
        public int InstallLibrary(LuaLibraryFunction function)
        {
            if (null == function) {
                Logger<ILuaRuntime>.E("Library entry function is null!");
                return -1;
            }

            return function(this.luaState);
        }

        private void _Terminate()
        {
            Lua.LuaClose(this.luaState);
        }

        [MonoPInvokeCallback(typeof(LuaNativeFunction))]
        private static int _PanicCallback(LuaState luaState)
        {
            string reason = string.Format("unprotected error in call to Lua API ({0})", LuaLib.LuaToString(luaState, -1));
            throw new Exception(reason);
        }

        // Replace 'print' module.
        // - mouguangyi 2016.07.26
        [MonoPInvokeCallback(typeof(LuaNativeFunction))]
        private static int _PrintCallback(LuaState luaState)
        {
            try {
                var n = LuaLib.LuaGetTop(luaState);
                var builder = new StringBuilder();
                for (var i = 1; i <= n; ++i) {
                    if (LuaLib.LuaIsString(luaState, i)) {
                        builder.Append(LuaLib.LuaToString(luaState, i));
                        builder.Append(" ");
                    }
                }
                Logger<ILuaRuntime>.L(builder.ToString());
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }

            return 0;
        }

        // Replace 'dofile' module.
        // - mouguangyi 2016.08.11
        [MonoPInvokeCallback(typeof(LuaNativeFunction))]
        private static int _DoFileCallback(LuaState luaState)
        {
            // Use asset manager to replace original dofile process.
            // - mouguangyi
            try {
                var fileName = LuaLib.LuaToString(luaState, 1);
                using (var asset = ServiceCenter.GetService<IAssetManager>().Load(fileName + ".txt", AssetType.BYTES)) {
                    var chunk = asset.Cast<byte[]>();
                    if (null == chunk) {
                        throw new Exception("Can't load lua script: " + fileName);
                    } else {
                        int oldTop = LuaLib.LuaGetTop(luaState);
                        if (0 == LuaLib.LuaLLoadBuffer(luaState, chunk, fileName)) {
                            if (0 != LuaLib.LuaPCall(luaState, 0, -1, 0)) {
                                _ThrowExceptionFromError(luaState, oldTop);
                            }
                        } else {
                            _ThrowExceptionFromError(luaState, oldTop);
                        }
                    }
                }
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }

            return 0;
        }

        // Replace 'searcher' module
        // - mouguangyi 2016.07.26
        [MonoPInvokeCallback(typeof(LuaNativeFunction))]
        private static int _SearcherCallback(LuaState luaState)
        {
            try {
                var fileName = LuaLib.LuaToString(luaState, 1);
                using (var asset = ServiceCenter.GetService<IAssetManager>().Load(fileName + ".txt", AssetType.BYTES)) {
                    var chunk = asset.Cast<byte[]>();
                    if (null == chunk) {
                        throw new Exception("Can't load lua script: " + fileName);
                    } else if (0 != LuaLib.LuaLLoadBuffer(luaState, chunk, fileName)) {
                        var err = LuaLib.LuaToString(luaState, -1);
                        throw new Exception(err);
                    }
                }
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }

            return 1;
        }

        private object[] _PopValues(LuaState luaState, int oldTop)
        {
            int newTop = LuaLib.LuaGetTop(luaState);

            if (oldTop == newTop) {
                return null;
            } else {
                var returnValues = new List<object>();
                for (int i = oldTop + 1; i <= newTop; i++) {
                    returnValues.Add(LuaExecuter._ParseLuaValue(luaState, i));
                }

                LuaLib.LuaSetTop(luaState, oldTop);
                return returnValues.ToArray();
            }
        }

        private object _GetObject(string[] remainingPath)
        {
            object returnValue = null;

            for (int i = 0; i < remainingPath.Length; i++) {
                LuaLib.LuaPushString(luaState, remainingPath[i]);
                LuaLib.LuaGetTable(luaState, -2);
                returnValue = LuaExecuter._ParseLuaValue(luaState, -1);

                if (returnValue == null) {
                    break;
                }
            }

            return returnValue;
        }

        private static void _ThrowExceptionFromError(LuaState luaState, int oldTop)
        {
            object err = LuaExecuter._ParseLuaValue(luaState, -1);
            LuaLib.LuaSetTop(luaState, oldTop);

            // A pre-wrapped exception - just rethrow it (stack trace of InnerException will be preserved)
            var luaEx = err as Exception;
            if (null != luaEx) {
                throw luaEx;
            }

            // A non-wrapped Lua error (best interpreted as a string) - wrap it and throw it
            if (null == err) {
                err = "Unknown Lua Error";
            }

            throw new Exception(err.ToString());
        }

        private LuaState luaState;
        private LuaNativeFunction panicCallback = null;
        private LuaNativeFunction printCallback = null;
        private LuaNativeFunction dofileCallback = null;
        private LuaNativeFunction searcherCallback = null;
        private LuaNativeFunction collectCallback = null;
    }
}
