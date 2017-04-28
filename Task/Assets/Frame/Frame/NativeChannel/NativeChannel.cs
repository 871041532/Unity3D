// -----------------------------------------------------------------
// File:    NativeChannel.cs
// Author:  mouguangyi
// Date:    2016.09.29
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GameBox.Service.NativeChannel
{
    class NativeChannel : INativeChannel
    {
        #region IService
        public string Id
        {
            get {
                return "com.giant.service.nativechannel";
            }
        }

        public void Run(IServiceRunner runner)
        {
            runner.Ready(_Terminate);
        }

        public void Pulse(float delta)
        {
            var json = NativeBridge.Receive();
            if (!string.IsNullOrEmpty(json)) {
               // var invoke = SimpleJson.SimpleJson.DeserializeObject<InvokeProtocol>(json);
               // var proxy = this.nativeProxies[invoke.Token];
                //if (null != proxy) {
                 //   proxy._Receive(invoke.Method, invoke.Args);
              //  }
            }
        }
        #endregion
        #region INativeChannel
        public void Call(NativeModule module, string method, params object[] args)
        {
            using (var proxy = Connect(module, new InnerManagedProxy())) {
                proxy.Call(method, args);
            }
        }

        public INativeProxy Connect<T>(NativeModule module, T proxy) where T : class
        {
            var token = this.nativeProxies.Count;
            var nativeProxy = new NativeProxy(this, proxy, token);
            this.nativeProxies.Add(nativeProxy);
            var protocol = new ConnnectProtocol() {
                Token = token,
                Module = _GetTargetModule(module),
            };
            //var message = SimpleJson.SimpleJson.SerializeObject(protocol);
          //  NativeBridge.Connect(message);

            return nativeProxy;
        }
        #endregion

        internal void _Send(int token, string method, object[] args)
        {
            var protocol = new InvokeProtocol() {
                Token = token,
                Method = method,
                Args = args,
                ArgTypes = _GetArgTypes(args),
            };
           // var message = SimpleJson.SimpleJson.SerializeObject(protocol);
           // NativeBridge.Send(message);
        }

        internal void _Disconnect(int token)
        {
            var protocol = new DisconnnectProtocol() {
                Token = token,
            };
           // var message = SimpleJson.SimpleJson.SerializeObject(protocol);
           // NativeBridge.Disconnect(message);
        }

        private void _Terminate()
        { }

        private string _GetTargetModule(NativeModule module)
        {
            switch (Application.platform) {
            case RuntimePlatform.IPhonePlayer:
                return string.IsNullOrEmpty(module.IOS) ? module.Default : module.IOS;
            case RuntimePlatform.Android:
                return string.IsNullOrEmpty(module.Android) ? module.Default : module.Android;
            default:
                return string.IsNullOrEmpty(module.Default) ? string.Empty : module.Default;
            }
        }

        private string[] _GetArgTypes(object[] args)
        {
            string[] argTypes = null;
            if (null != args && args.Length > 0) {
                argTypes = new string[args.Length];
                for (var i = 0; i < args.Length; ++i) {
                    argTypes[i] = args[i].GetType().Name;
                }
            }
            else
            {
                argTypes = new string[0];
            }

            return argTypes;
        }

        private class InnerManagedProxy
        { }

        private class ConnnectProtocol
        {
            public int Token;
            public string Module;
        }

        private class InvokeProtocol
        {
            public int Token;
            public string Method;
            public object[] Args;
            public string[] ArgTypes;
        }

        private class DisconnnectProtocol
        {
            public int Token;
        }

        private List<NativeProxy> nativeProxies = new List<NativeProxy>();
    }
}