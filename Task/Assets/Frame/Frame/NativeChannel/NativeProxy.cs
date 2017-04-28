// -----------------------------------------------------------------
// File:    NativeProxy.cs
// Author:  mouguangyi
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Reflection;

namespace GameBox.Service.NativeChannel
{
    class NativeProxy : INativeProxy
    {
        public NativeProxy(NativeChannel channel, object proxy, int token)
        {
            this.channel = channel;
            this.proxy = proxy;
            this.token = token;
        }

        public void Dispose()
        {
            Disconnect();
            this.channel = null;
            this.proxy = null;
        }

        public void Call(string method, params object[] args)
        {
            this.channel._Send(this.token, method, args);
        }

        public void Disconnect()
        {
            this.channel._Disconnect(this.token);
        }

        internal void _Receive(string method, object[] args)
        {
            if (null != this.proxy) {
                var classType = this.proxy.GetType();
                var methodInfo = classType.GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (null != methodInfo) {
                    try {
                        methodInfo.Invoke(this.proxy, args);
                    } catch (Exception e) {
                        Logger<INativeChannel>.X(e);
                    }
                } else {
                    Logger<INativeChannel>.E(string.Format("CANNOT find {0} in {1} class!", method, classType.Name));
                }
            }
        }

        private NativeChannel channel = null;
        private object proxy = null;
        private int token = -1;
    }
}