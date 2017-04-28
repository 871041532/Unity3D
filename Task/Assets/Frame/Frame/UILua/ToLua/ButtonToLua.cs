// -----------------------------------------------------------------
// File:    ButtonToLua.cs
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
    class ButtonToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(ButtonToLua.LuaAddClick);
            luaRuntime.RegLuaBridgeFunction(ButtonToLua.LuaGetInteractable);
            luaRuntime.RegLuaBridgeFunction(ButtonToLua.LuaSetInteractable);
        }


        [LuaBridge("button_addClick", 2, 0)]
        public static void LuaAddClick(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var button = parameters[0] as IButton;
            var handler = parameters[1];
            if (null != button) {
                button.OnClick(() =>
                {
                    if (null != handler) {
                        executer.Call(handler);
                    }
                });
            }
        }

        [LuaBridge("button_getInteractable", 1, 1)]
        public static void LuaGetInteractable(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var button = parameters[0] as IButton;
            if (null != button) {
                executer.PushResult(button.Interactable);
            } else {
                executer.PushResult(false);
            }
        }

        [LuaBridge("button_setInteractable", 2, 0)]
        public static void LuaSetInteractable(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var button = parameters[0] as IButton;
            var interactable = Convert.ToBoolean(parameters[1]);
            if (null != button) {
                button.Interactable = interactable;
            }
        }
    }
}