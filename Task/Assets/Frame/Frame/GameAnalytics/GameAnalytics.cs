// -----------------------------------------------------------------
// File:    GameAnalytics.cs
// Author:  liuwei
// Date:    2017.03.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using System.Collections.Generic;

namespace GameBox.Service.GameAnalytics
{
    sealed class GameAnalytics : IGameAnalytics
    {
        public string Id
        {
            get {
                return "com.giant.service.gameanalytics";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServiceTask<INativeChannel>().Start().Continue(task =>
            {
                var nativeChannel = task.Result as INativeChannel;
                this.proxy = nativeChannel.Connect(new NativeModule
                {
                    IOS = "IOSGameAnalyticsWrapper",
                    Android = "com.giant.gameanalytics.AndroidGameAnalyticsWrapper",
                }, this);

                var appId = runner.GetArgs<string>("AppId");//ÓÎÏ·id
                var appChannel = runner.GetArgs<string>("AppChannel");//ÓÎÏ·ÇþµÀºÅ
                this.proxy.Call("init", appId, appChannel);

                runner.Ready(_Terminate);
                return null;
            });
        }

        public void OnEvent(string eventId, Dictionary<string, object> dictionary)
        {
            this.proxy.Call("onEvent", eventId, dictionary);
        }

        public void Pulse(float delta)
        { }

        private void _Terminate()
        { }

        private INativeProxy proxy;
    }
}