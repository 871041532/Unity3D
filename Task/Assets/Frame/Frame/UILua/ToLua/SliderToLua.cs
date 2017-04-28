// -----------------------------------------------------------------
// File:    SliderToLua.cs
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
    class SliderToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(SliderToLua.LuaGetValue);
            luaRuntime.RegLuaBridgeFunction(SliderToLua.LuaSetValue);
        }


        [LuaBridge("slider_getValue", 1, 1)]
        public static void LuaGetValue(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var slider = parameters[0] as ISlider;
            if (null != slider) {
                executer.PushResult(slider.Value);
            } else {
                executer.PushResult(0);
            }
        }

        [LuaBridge("slider_setValue", 2, 0)]
        public static void LuaSetValue(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var slider = parameters[0] as ISlider;
            if (null != slider && null != parameters[1]) {
                slider.Value = Convert.ToSingle(parameters[1]);
            }
        }
    }
}