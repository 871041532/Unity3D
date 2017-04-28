// -----------------------------------------------------------------
// File:    WindowToLua.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.LuaRuntime;
using GameBox.Service.UI;

namespace GameBox.Service.UIToLua
{
    class WindowToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(WindowToLua.LuaCloseWindow);
        }

        [LuaBridge("window_close", 1, 0)]
        public static void LuaCloseWindow(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var window = parameters[0] as IWindow;
            if (null != window) {
                window.Close();
            }
        }
    }
}