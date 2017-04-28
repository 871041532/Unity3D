// -----------------------------------------------------------------
// File:    UISystemToLua.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.LuaRuntime;
using GameBox.Service.UI;
using System;

namespace GameBox.Service.UIToLua
{
    class UISystemToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(UISystemToLua.LuaCreateWindow);
            luaRuntime.RegLuaBridgeFunction(UISystemToLua.LuaCreateWindowAsync);
        }

        [LuaBridge("manager_createWindow", 3, 1)]
        public static void LuaCreateWindow(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var manager = parameters[0] as IUISystem;
            if (null != manager && null != parameters[1] && null != parameters[2]) {
                var path = parameters[1].ToString();
                var layer = Convert.ToInt32(parameters[2]);
                var window = manager.CreateWindow(path, layer);
                executer.PushResult(window);
            } else {
                executer.PushResult(null);
            }
        }

        [LuaBridge("manager_createWindowAsync", 4, 0)]
        public static void LuaCreateWindowAsync(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var manager = parameters[0] as IUISystem;
            if (null != manager && null != parameters[1] && null != parameters[2]) {
                var path = parameters[1].ToString();
                var layer = Convert.ToInt32(parameters[2]);
                var callback = parameters[3];
                var target = (parameters.Length > 4 ? parameters[4] : null);
                manager.CreateWindowAsync(path, layer, window =>
                {
                    if (null != callback) {
                        if (null == target) {
                            executer.Call(callback, new object[] { window });
                        } else {
                            executer.Call(callback, new object[] { target, window });
                        }
                    }
                });
            }
        }
    }
}