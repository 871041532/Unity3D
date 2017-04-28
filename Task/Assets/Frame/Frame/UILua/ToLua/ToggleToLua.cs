// -----------------------------------------------------------------
// File:    ToggleToLua.cs
// Author:  mouguangyi
// Date:    2017.01.17
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.LuaRuntime;
using GameBox.Service.UI;
using System;

namespace GameBox.Service.UIToLua
{
    class ToggleToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(ToggleToLua.LuaGetOn);
            luaRuntime.RegLuaBridgeFunction(ToggleToLua.LuaSetOn);
            luaRuntime.RegLuaBridgeFunction(ToggleToLua.LuaOnChange);
        }

        [LuaBridge("toggle_getOn", 1, 1)]
        public static void LuaGetOn(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var toggle = parameters[0] as IToggle;
            if (null != toggle) {
                executer.PushResult(toggle.On);
            } else {
                executer.PushResult(false);
            }
        }

        [LuaBridge("toggle_setOn", 2, 0)]
        public static void LuaSetOn(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var toggle = parameters[0] as IToggle;
            if (null != toggle && null != parameters[1]) {
                toggle.On = Convert.ToBoolean(parameters[1]);
            }
        }

        [LuaBridge("toggle_onChange", 2, 0)]
        public static void LuaOnChange(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var toggle = parameters[0] as IToggle;
            var handler = parameters[1];
            if (null != toggle) {
                toggle.OnChange(isOn =>
                {
                    if (null != handler) {
                        executer.Call(handler, new object[] { isOn });
                    }
                });
            }
        }
    }
}