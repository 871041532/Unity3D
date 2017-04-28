// -----------------------------------------------------------------
// File:    Getui.cs
// Author:  mouguangyi
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;

namespace GameBox.Service.Getui
{
    sealed class Getui : IGetui
    {
        public string Id
        {
            get {
                return "com.giant.service.getui";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServiceTask<INativeChannel>().Start().Continue(task =>
            {
                var appId = runner.GetArgs<string>("AppId");
                var appKey = runner.GetArgs<string>("AppKey");
                var appSecret = runner.GetArgs<string>("AppSecret");
                var nativeChannel = task.Result as INativeChannel;
                nativeChannel.Call(new NativeModule {
                    IOS = "GetuiWrapper",
                    Android = "com.giant.getui.GetuiWrapper",
                }, "init", appId, appKey, appSecret);

                Logger<IGetui>.L("Getui initialize complete.");

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