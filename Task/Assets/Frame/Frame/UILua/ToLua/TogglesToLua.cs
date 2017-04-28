// -----------------------------------------------------------------
// File:    TogglesToLua.cs
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
    class TogglesToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(TogglesToLua.LuaGetToggleOn);
            luaRuntime.RegLuaBridgeFunction(TogglesToLua.LuaSetToggleOn);
            luaRuntime.RegLuaBridgeFunction(TogglesToLua.LuaAddToggleChange);
        }

        [LuaBridge("toggles_getOn", 1, 1)]
        public static void LuaGetToggleOn(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var toggles = parameters[0] as IToggles;
            if (null != toggles) {
                executer.PushResult(toggles.ToggleOn);
            } else {
                executer.PushResult(-1);
            }
        }

        [LuaBridge("toggles_setOn", 2, 0)]
        public static void LuaSetToggleOn(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var toggles = parameters[0] as IToggles;
            if (null != toggles && null != parameters[1]) {
                toggles.ToggleOn = Convert.ToInt32(parameters[1]);
            }
        }

        [LuaBridge("toggles_addChange", 2, 0)]
        public static void LuaAddToggleChange(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var toggles = parameters[0] as IToggles;
            var handler = parameters[1];
            if (null != toggles) {
                toggles.AddToggleChange(() =>
                {
                    if (null != handler) {
                        executer.Call(handler);
                    }
                });
            }
        }
    }
}