// -----------------------------------------------------------------
// File:    GiantLightGame.cs
// Author:  mouguangyi
// Date:    2016.07.18
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.GiantLightServer;
using System.Collections.Generic;

namespace GameBox.Service.GiantLightFramework
{
    class GiantLightGame : IGiantGame
    {
        public GiantLightGame()
        { }

        public string Id
        {
            get {
                return "com.giant.service.giantlightframework";
            }
        }

        public void Run(IServiceRunner runner)
        {
            this.serverIP = runner.GetArgs<string>("GiantServerIP");
            this.serverPort = runner.GetArgs<int>("GiantServerPort");

            new ServiceTask("com.giant.service.giantlightserver").Start()
            .Continue(task =>
            {
                this.server = task.Result as IGiantLightServer;
                runner.Ready(_Terminate);
                return null;
            })
            .Continue(task =>
            {
               // this.server.Connect(this.serverIP, this.serverPort, this);
                Logger<IGiantGame>.L("Start to connect giant server...");
                return null;
            });
        }

        public void Pulse(float delta)
        {
            if (null != this.scene) {
                this.scene.Update(delta);
            }
        }

        public void GotoScene(GiantLightScene scene)
        {
            if (null != this.scene) {
                this.scene.Exit(this);
            }

            this.scene = scene;

            if (null != this.scene) {
                this.scene.Enter(this);
            }
        }

        public void SetUserData<T>(string key, T data)
        {
            this.userDatas.Add(key, data);
        }

        public T GetUserData<T>(string key)
        {
            object data = null;
            if (this.userDatas.TryGetValue(key, out data)) {
                return (T)data;
            } else {
                return default(T);
            }
        }

        public bool IsConnected
        {
            get {
                return this.server.Connected;
            }
        }

        public void Disconnect()
        {
            if (null != this.scene) {
                this.scene.Disconnect();
            }
        }

        public void SendRequest(uint id, string service, string method, byte[] content)
        {
           // this.server.SendRequest(id, service, method, content);
        }

        public void SendResponse(uint id, byte[] content)
        {
            this.server.SendResponse(id, content);
        }

        public void OnDisconnect()
        {}

        public bool PushRequest(uint id, string service, string method, byte[] content)
        {
            return (null != this.scene ? this.scene.PushRequest(id, service, method, content) : false);
        }

        public bool PushResponse(uint id, byte[] content)
        {
            return (null != this.scene ? this.scene.PushResponse(id, content) : false);
        }

        private void _Terminate()
        { }

        private GiantLightScene scene = null;
        private Dictionary<string, object> userDatas = new Dictionary<string, object>();
        private string serverIP = null;
        private int serverPort = -1;
        private IGiantLightServer server = null;
    }
}