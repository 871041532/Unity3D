// -----------------------------------------------------------------
// File:    LuaStruct.cs
// Author:  mouguangyi
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.LuaRuntime.Struct
{
    class LuaStruct : ILuaStruct
    {
        public string Id
        {
            get {
                return "com.giant.service.luaruntime.struct";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServiceTask("com.giant.service.luaruntime").Start().Continue(task =>
            {
                var luaRuntime = task.Result as ILuaRuntime;
                luaRuntime.InstallLibrary(NativeMethods.LuaOpenStruct);
                runner.Ready(_Terminate);

                return null;
            });
        }

        public void Pulse(float delta)
        { }

        private void _Terminate()
        { }
    }
}