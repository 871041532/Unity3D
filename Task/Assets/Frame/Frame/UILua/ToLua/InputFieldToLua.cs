// -----------------------------------------------------------------
// File:    InputFieldToLua.cs
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
    class InputFieldToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaGetText);
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaSetText);
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaGetInputType);
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaSetInputType);
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaGetKeyboardType);
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaSetKeyboardType);
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaGetLineType);
            luaRuntime.RegLuaBridgeFunction(InputFieldToLua.LuaSetLineType);
        }

        [LuaBridge("input_getText", 1, 1)]
        public static void LuaGetText(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input) {
                executer.PushResult(input.Text);
            } else {
                executer.PushResult(null);
            }
        }

        [LuaBridge("input_setText", 2, 0)]
        public static void LuaSetText(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input && null != parameters[1]) {
                input.Text = parameters[1].ToString();
            }
        }

        [LuaBridge("input_getInputType", 1, 1)]
        public static void LuaGetInputType(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input) {
                executer.PushResult(input.InputType);
            }
        }

        [LuaBridge("input_setInputType", 2, 0)]
        public static void LuaSetInputType(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input && null != parameters[1]) {
                input.InputType = Convert.ToInt32(parameters[1]);
            }
        }

        [LuaBridge("input_getKeyboardType", 1, 1)]
        public static void LuaGetKeyboardType(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input) {
                executer.PushResult(input.KeyboardType);
            }
        }

        [LuaBridge("input_setKeyboardType", 2, 0)]
        public static void LuaSetKeyboardType(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input && null != parameters[1]) {
                input.KeyboardType = Convert.ToInt32(parameters[1]);
            }
        }

        [LuaBridge("input_getLineType", 1, 1)]
        public static void LuaGetLineType(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input) {
                executer.PushResult(input.LineType);
            }
        }

        [LuaBridge("input_setLineType", 2, 0)]
        public static void LuaSetLineType(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var input = parameters[0] as IInputField;
            if (null != input && null != parameters[1]) {
                input.LineType = Convert.ToInt32(parameters[1]);
            }
        }
    }
}