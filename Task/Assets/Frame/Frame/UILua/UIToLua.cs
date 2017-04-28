// -----------------------------------------------------------------
// File:    UIToLua.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.LuaRuntime;

namespace GameBox.Service.UIToLua
{
    class UIToLua : IUIToLua
    {
        public string Id
        {
            get {
                return "com.giant.service.uitolua";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServicesTask(new string[]{
                "com.giant.service.uisystem",
                "com.giant.service.luaruntime",
            }).Start().Continue(task =>
            {
                var services = task.Result as IService[];
                var luaRuntime = services[1] as ILuaRuntime;

                UISystemToLua.RegLuaBridgeFunction(luaRuntime);
                ElementToLua.RegLuaBridgeFunction(luaRuntime);
                ButtonToLua.RegLuaBridgeFunction(luaRuntime);
                DropdownToLua.RegLuaBridgeFunction(luaRuntime);
                ImageToLua.RegLuaBridgeFunction(luaRuntime);
                ImageAnimationToLua.RegLuaBridgeFunction(luaRuntime);
                InputFieldToLua.RegLuaBridgeFunction(luaRuntime);
                LabelToLua.RegLuaBridgeFunction(luaRuntime);
                ScrollViewToLua.RegLuaBridgeFunction(luaRuntime);
                SliderToLua.RegLuaBridgeFunction(luaRuntime);
                ToggleToLua.RegLuaBridgeFunction(luaRuntime);
                TogglesToLua.RegLuaBridgeFunction(luaRuntime);
                WindowToLua.RegLuaBridgeFunction(luaRuntime);

                runner.Ready(_Terminate);

                return null;
            });
        }

        public void Pulse(float delata)
        { }

        private void _Terminate()
        { }
    }
}
