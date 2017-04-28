// -----------------------------------------------------------------
// File:    LuaProtocolBuffer.cs
// Author:  gexiaoyi
// Date:    2016.08.11
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.LuaRuntime.ProtocolBuffer
{
    class LuaProtocolBuffer : ILuaProtocolBuffer
    {
        public string Id
        {
            get {
                return "com.giant.service.luaruntime.protocolbuffer";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServiceTask("com.giant.service.luaruntime").Start().Continue(task =>
            {
                var luaRuntime = task.Result as ILuaRuntime;
                luaRuntime.InstallLibrary(NativeMethods.LuaOpenProtocolBuffer);

                // DoString()(��LoadString() tolua�������е���ProtoBuffer�йص�lua�ӿ�)
                // - gexiaoyi
                LuaWireformatModule.Install(luaRuntime);
                LuaTypecheckersModule.Install(luaRuntime);
                LuaEncoderModule.Install(luaRuntime);
                LuaDecoderModule.Install(luaRuntime);
                LuaListenerModule.Install(luaRuntime);
                LuaContainersModule.Install(luaRuntime);
                LuaDescriptorModule.Install(luaRuntime);
                LuaTextformatModule.Install(luaRuntime);
                LuaProtobufModule.Install(luaRuntime);

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