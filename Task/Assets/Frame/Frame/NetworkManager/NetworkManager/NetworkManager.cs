// -----------------------------------------------------------------
// File:    NetworkManager.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.ByteStorage;
using System.Net.Sockets;

namespace GameBox.Service.NetworkManager
{
    sealed class NetworkManager : INetworkManager
    {
        public string Id
        {
            get {
                return "com.giant.service.networkmanager";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServiceTask("com.giant.service.bytestorage").Start().Continue(task =>
            {
                this.storage = task.Result as IByteStorage;
                runner.Ready(_Terminate);
                Logger<INetworkManager>.L("NetworkManager service is ready.");
                return null;
            });
        }

        public void Pulse(float delta)
        { }

        public INetworkChannel Create(string type)
        {
            INetworkChannel channel = null;
            switch (type.ToLower()) {
            case "http":
                channel = new HttpChannel();
                break;
            case "tcp":
                channel = new SocketChannel(this.storage, ProtocolType.Tcp);
                break;
            case "udp":
                channel = new SocketChannel(this.storage, ProtocolType.Udp);
                break;
            default:
                break;
            }

            return channel;
        }

        private void _Terminate()
        {
            this.storage = null;
        }

        private IByteStorage storage = null;
    }
}