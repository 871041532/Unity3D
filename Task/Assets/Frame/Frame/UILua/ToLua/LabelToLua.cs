// -----------------------------------------------------------------
// File:    LabelToLua.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.LuaRuntime;
using GameBox.Service.UI;
using System;
using UnityEngine;

namespace GameBox.Service.UIToLua
{
    class LabelToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(LabelToLua.LuaGetText);
            luaRuntime.RegLuaBridgeFunction(LabelToLua.LuaSetText);
            luaRuntime.RegLuaBridgeFunction(LabelToLua.LuaGetTextColor);
            luaRuntime.RegLuaBridgeFunction(LabelToLua.LuaSetTextColor);
            luaRuntime.RegLuaBridgeFunction(LabelToLua.LuaGetFontSize);
            luaRuntime.RegLuaBridgeFunction(LabelToLua.LuaSetFontSize);
            luaRuntime.RegLuaBridgeFunction(LabelToLua.LuaSetHrefClick);
        }

        [LuaBridge("label_getText", 1, 1)]
        public static void LuaGetText(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var label = parameters[0] as ILabel;
            if (null != label) {
                executer.PushResult(label.Text);
            } else {
                executer.PushResult(null);
            }
        }

        [LuaBridge("label_setText", 2, 0)]
        public static void LuaSetText(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var label = parameters[0] as ILabel;
            if (null != label && null != parameters[1]) {
                label.Text = parameters[1].ToString();
            }
        }

        [LuaBridge("label_getTextColor", 1, 4)]
        public static void LuaGetTextColor(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var label = parameters[0] as ILabel;
            if (null != label) {
                var color = label.Color;
                executer.PushResult(color.r);
                executer.PushResult(color.g);
                executer.PushResult(color.b);
                executer.PushResult(color.a);
            } else {
                executer.PushResult(0);
                executer.PushResult(0);
                executer.PushResult(0);
                executer.PushResult(0);
            }
        }


        [LuaBridge("label_setTextColor", 5, 0)]
        public static void LuaSetTextColor(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var label = parameters[0] as ILabel;
            if (null != label && null != parameters[1] && null != parameters[2] && null != parameters[3]) {
                var r = Convert.ToSingle(parameters[1]);
                var g = Convert.ToSingle(parameters[2]);
                var b = Convert.ToSingle(parameters[3]);
                var a = Convert.ToSingle(parameters[4]);
                label.Color = new Color(r, g, b, a);
            }
        }

        [LuaBridge("label_getFontSize", 1, 1)]
        public static void LuaGetFontSize(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var label = parameters[0] as ILabel;
            if (null != label) {
                executer.PushResult(label.FontSize);
            } else {
                executer.PushResult(0);
            }
        }

        [LuaBridge("label_setFontSize", 2, 0)]
        public static void LuaSetFontSize(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var label = parameters[0] as ILabel;
            if (null != label && null != parameters[1]) {
                label.FontSize = Convert.ToInt32(parameters[1]);
            }
        }

        [LuaBridge("label_addClick", 2, 0)]
        public static void LuaSetHrefClick(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var label = parameters[0] as ILabel;
            var handler = parameters[1];
            if (null != label && null != handler && null != label.OnHrefClick)
            {
                label.OnClick(delegate (string hrefInfo)
                {
                    executer.Call(handler, hrefInfo);
                });
            }
        }
    }
}