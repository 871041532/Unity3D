// -----------------------------------------------------------------
// File:    ScrollViewToLua.cs
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
    class ScrollViewToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(ScrollViewToLua.LuaGetItems);
            luaRuntime.RegLuaBridgeFunction(ScrollViewToLua.LuaScrollToBottom);
            luaRuntime.RegLuaBridgeFunction(ScrollViewToLua.LuaScrollToTop);
        }

        [LuaBridge("scrollView_getItems", 2, 1)]
        public static void LuaGetItems(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var scrollView = parameters[0] as IScrollView;
            if (null != scrollView && null != parameters[1]) {
                executer.PushResult(scrollView.GetItems(Convert.ToInt32(parameters[1])));
            }
        }

        [LuaBridge("scrollView_scrollToBottom", 1, 0)]
        public static void LuaScrollToBottom(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var scrollView = parameters[0] as IScrollView;
            if (null != scrollView) {
                scrollView.ScrollToBottom();
            }
        }

        [LuaBridge("scrollView_scrollToTop", 1, 0)]
        public static void LuaScrollToTop(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var scrollView = parameters[0] as IScrollView;
            if (null != scrollView) {
                scrollView.ScrollToTop();
            }
        }
    }
}