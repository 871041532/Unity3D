// -----------------------------------------------------------------
// File:    LuaExecuter.cs
// Author:  mouguangyi
// Date:    2016.07.28
// Description:
//      
// -----------------------------------------------------------------
using AOT;
using GameBox.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameBox.Service.LuaRuntime
{
    using LuaState = KeraLuaLite.LuaState;
	using LuaNativeFunction = KeraLuaLite.LuaNativeFunction;
    using LuaLib = NLuaLite.LuaLib;
    using LuaTypes = NLuaLite.LuaTypes;

    class LuaExecuter : ILuaExecuter
    {
        public LuaExecuter(LuaRuntime runtime, LuaBridgeFunction function)
        {
            this.runtime = runtime;
            this.function = function;
            var attributes = function.GetInvocationList()[0].Method.GetCustomAttributes(typeof(LuaBridgeAttribute), false);
            if (attributes.Length > 0) {
                var luaBridgeAttribute = attributes[0] as LuaBridgeAttribute;
                this.name = luaBridgeAttribute.Name;
                this.input = luaBridgeAttribute.Input;
                this.output = luaBridgeAttribute.Output;
            } else {
                Logger<ILuaRuntime>.E("The function is missing LuaBridgeAttribute definition!");
            }

            this.index = LuaExecuter.executers.Count;
            LuaExecuter.executers.Add(this);
        }

        public object[] PopParameters()
        {
            var n = LuaLib.LuaGetTop(this.luaState);
            if (n < this.input) {
                Logger<ILuaRuntime>.W("Method input parameter count is incorrect since definition is " + this.input + ", but only" + n + " is input!");
            }

            if (n > 0) {
                var parameters = new object[n];
                for (var i = 0; i < n; ++i) {
                    parameters[i] = _ParseLuaValue(this.luaState, i + 1);
                }

                return parameters;
            } else {
                return null;
            }
        }

        public void PushResult(object result)
        {
            _PushObject(this.luaState, result);
            ++this.execOutput;
        }

        public object[] Call(object function, params object[] args)
        {
            return this.runtime.Call(function, args);
        }

        public string Name
        {
            get {
                return this.name;
            }
        }

        public int Index
        {
            get {
                return this.index;
            }
        }

        private LuaRuntime runtime = null;
        private LuaBridgeFunction function = null;
        private string name = null;
        private uint input = 0;
        private uint output = 0;
        private int index = 0;
        private LuaState luaState;
        private int execOutput = 0;

        [MonoPInvokeCallback(typeof(LuaNativeFunction))]
        internal static int _Execute(LuaState luaState)
        {
            var index = (int)LuaLib.LuaToNumber(luaState, LuaLib.LuaNetUpValueIndex(1));
            if (index >= 0 && index < LuaExecuter.executers.Count) {
                var executer = LuaExecuter.executers[index];
                executer.luaState = luaState;
                executer.execOutput = 0;
                executer.function(executer);
                if (executer.execOutput != executer.output) {
                    Logger<ILuaRuntime>.W("Method return count is incorrect since the definition is " + executer.output + ", but " + executer.execOutput + " is returned!");
                }
                return (int)executer.execOutput;
            }
            else {
                Logger<ILuaRuntime>.E("Can't find related C# function!");
                return 0;
            }
        }


        [MonoPInvokeCallback(typeof(LuaNativeFunction))]
        internal static int _CollectObject(LuaState luaState)
        {
            var allocId = LuaLib.LuaNetRawNetObj(luaState, 1);
            object o = null;
            if (LuaExecuter.backObjects.TryGetValue(allocId, out o)) {
                if (0 == (LuaExecuter.objectRefs[o] -= 1)) {
                    LuaExecuter.objects.Remove(o);
                    LuaExecuter.objectRefs.Remove(o);
                    LuaExecuter.backObjects.Remove(allocId);
                }
            }

            return 0;
        }

        internal static object _ParseLuaValue(LuaState luaState, int index)
        {
            try {
                var t = LuaLib.LuaType(luaState, index);
                switch (t) {
                case LuaTypes.Nil:
                    return null;
                case LuaTypes.Boolean:
                    return LuaLib.LuaToBoolean(luaState, index);
                case LuaTypes.Number:
                    return LuaLib.LuaToNumber(luaState, index);
                case LuaTypes.String:
                    return new LuaString(LuaLib.LuaToBytes(luaState, index));
                case LuaTypes.Function:
                    LuaLib.LuaPushValue(luaState, index);
                    int reference = LuaLib.LuaRef(luaState, 1);
                    return (-1 == reference ? null : new LuaFunction(reference));
                case LuaTypes.UserData:
                    var allocId = LuaLib.LuaNetRawNetObj(luaState, index);
                    if (LuaExecuter.backObjects.ContainsKey(allocId)) {
                        return LuaExecuter.backObjects[allocId];
                    } else {
                        return null;
                    }
                case LuaTypes.Table:
                    LuaLib.LuaPushValue(luaState, index);
                    reference = LuaLib.LuaRef(luaState, 1);
                    if (-1 == reference) {
                        return null;
                    }
                    return new LuaTable(luaState, reference);
                }
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }

            return null;
        }

        internal static void _PushObject(LuaState luaState, object result)
        {
            try {
                if (null == result) {
                    LuaLib.LuaPushNil(luaState);
                } else if (result is LuaBase) {         // LuaFunction or LuaTable
                    LuaLib.LuaGetRef(luaState, (result as LuaBase).Reference);
                } else if (result is LuaString) {       // LuaString
                    var bytes = (result as LuaString).ToBytes();
                    LuaLib.LuaPushLString(luaState, bytes, bytes.Length);
                } else {
                    var t = result.GetType();
                    if (t.IsValueType) {
                        if (t == typeof(bool)) {        // bool
                            LuaLib.LuaPushBoolean(luaState, (bool)result);
                        } else if (t.IsPrimitive) {     // number
                            LuaLib.LuaPushNumber(luaState, _ConvertToDouble(result));
                        } else if (t.IsEnum) {          // enum

                        } else {                        // structure
                            var size = Marshal.SizeOf(result);
                            var bytes = new byte[size];
                            var ptr = Marshal.AllocHGlobal(size);
                            Marshal.StructureToPtr(result, ptr, false);
                            Marshal.Copy(ptr, bytes, 0, size);
                            Marshal.FreeHGlobal(ptr);
                            LuaLib.LuaPushLString(luaState, bytes, size);
                        }
                    } else if (t == typeof(string)) {   // string
                        LuaLib.LuaPushString(luaState, (string)result);
                    } else if (t.IsArray) {
                        if ("Byte[]" == t.Name) {       // byte[] will be transferred through lstring
                            var bytes = result as byte[];
                            LuaLib.LuaPushLString(luaState, bytes, bytes.Length);
                        } else {                        // other array type
                            LuaLib.LuaNewTable(luaState);
                            var arr = result as Array;
                            for (var i = 0; i < arr.Length; ++i) {
                                LuaLib.LuaPushNumber(luaState, i + 1);
                                _PushObject(luaState, arr.GetValue(i));
                                LuaLib.LuaSetTable(luaState, -3);
                            }
                        }
                    } else if (result is IDictionary) { // KeyPair<TKey, TValue> related
                        LuaLib.LuaNewTable(luaState);
                        var dict = result as IDictionary;
                        var keys = dict.Keys;
                        foreach (var key in keys) {
                            _PushObject(luaState, key);
                            _PushObject(luaState, dict[key]);
                            LuaLib.LuaSetTable(luaState, -3);
                        }
                    } else if (result is IEnumerable) { // Array related
                        LuaLib.LuaNewTable(luaState);
                        var enumerable = result as IEnumerable;
                        var index = 0;
                        foreach (var v in enumerable) {
                            LuaLib.LuaPushNumber(luaState, index);
                            _PushObject(luaState, v);
                            LuaLib.LuaSetTable(luaState, -3);
                            ++index;
                        }
                    } else {                            // user data
                        int allocId = -1;
                        if (!LuaExecuter.objects.TryGetValue(result, out allocId)) {
                            allocId = (++LuaExecuter.objectAllocId);
                            LuaExecuter.objects.Add(result, allocId);
                            LuaExecuter.objectRefs.Add(result, 1);
                            LuaExecuter.backObjects.Add(allocId, result);
                        } else {
                            LuaExecuter.objectRefs[result] += 1;
                        }

                        LuaLib.LuaNetNewUData(luaState, allocId);
                        LuaLib.LuaLGetMetatable(luaState, "luaruntime");
                        LuaLib.LuaSetMetatable(luaState, -2);
                    }
                }
            } catch (Exception e) {
                Logger<ILuaRuntime>.X(e);
            }
        }

        private static double _ConvertToDouble(object value)
        {
            var t = value.GetType();
            if (t == typeof(double) || t == typeof(float)) {
                return Convert.ToDouble(value);
            } else if (t == typeof(short)) {
                return (double)Convert.ToInt16(value);
            } else if (t == typeof(ushort)) {
                return (double)Convert.ToUInt16(value);
            } else if (t == typeof(int)) {
                return (double)Convert.ToInt32(value);
            } else if (t == typeof(uint)) {
                return (double)Convert.ToUInt32(value);
            } else if (t == typeof(long)) {
                return (double)Convert.ToInt64(value);
            } else if (t == typeof(ulong)) {
                return (double)Convert.ToUInt64(value);
            } else if (t == typeof(byte)) {
                return (double)Convert.ToByte(value);
            } else if (t == typeof(sbyte)) {
                return (double)Convert.ToSByte(value);
            } else if (t == typeof(char)) {
                return (double)Convert.ToChar(value);
            }

            return 0;
        }

        private static List<LuaExecuter> executers = new List<LuaExecuter>();
        private static int objectAllocId = 0;
        private static Dictionary<object, int> objects = new Dictionary<object, int>();
        private static Dictionary<object, int> objectRefs = new Dictionary<object, int>();
        private static Dictionary<int, object> backObjects = new Dictionary<int, object>();
    }
}