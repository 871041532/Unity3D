// -----------------------------------------------------------------
// File:    DropdownToLua.cs
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
    class DropdownToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(DropdownToLua.LuaGetValue);
            luaRuntime.RegLuaBridgeFunction(DropdownToLua.LuaAddOption);
            luaRuntime.RegLuaBridgeFunction(DropdownToLua.LuaClearOptions);
            luaRuntime.RegLuaBridgeFunction(DropdownToLua.LuaAddClick);
            luaRuntime.RegLuaBridgeFunction(DropdownToLua.LuaGetOption);
        }

        [LuaBridge("dropdown_getValue", 1, 1)]
        public static void LuaGetValue(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var dropdown = parameters[0] as IDropDown;
            if (null != dropdown) {
                executer.PushResult(dropdown.Value);
            } else {
                executer.PushResult(0);
            }
        }

        [LuaBridge("dropdown_addOption", 2, 0)]
        public static void LuaAddOption(ILuaExecuter executor)
        {
            var parameters = executor.PopParameters();
            var dropdown = parameters[0] as IDropDown;
            if (null != dropdown && null != parameters[1]) {
                var option = parameters[1].ToString();
                dropdown.AddOption(option);
            }
        }

        [LuaBridge("dropdown_clearOptions", 1, 0)]
        public static void LuaClearOptions(ILuaExecuter executor)
        {
            var parameters = executor.PopParameters();
            var dropdown = parameters[0] as IDropDown;
            if (null != dropdown) {
                dropdown.ClearOptions();
            }
        }

        [LuaBridge("dropdown_addClick", 2, 0)]
        public static void LuaAddClick(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var dropdown = parameters[0] as IDropDown;
            var handler = parameters[1];
            if (null != dropdown) {
                dropdown.OnClick((index) =>
                {
                    if (null != handler) {
                        executer.Call(handler, new object[] { index });
                    }
                });
            }
        }

        [LuaBridge("dropdown_getOption", 2, 1)]
        public static void LuaGetOption(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var dropdown = parameters[0] as IDropDown;
            if (null != dropdown && null != parameters[1]) {
                var index = Convert.ToInt32(parameters[1]);
                executer.PushResult(dropdown.GetOption(index));
            }
        }
    }
}