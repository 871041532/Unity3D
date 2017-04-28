// -----------------------------------------------------------------
// File:    GiantLightProxy.cs
// Author:  mouguangyi
// Date:    2017.03.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.ByteStorage;
using System;
using System.Collections.Generic;

namespace GameBox.Service.GiantLightServer
{
    sealed class GiantLightProxy : IGiantLightProxy
    {
        //public GiantLightProxy(GiantLightServer server, string service, ServiceType type)
        //{
        //    this.server = server;
        //    this.service = service;
        //    this.type = type;
        //}

        public void Dispose()
        {
            //if (null != this.server) {
            //    this.server._DeleteProxy(this.service);
            //    this.server = null;
            //}
        }

        public void CommonRegister<T>(string method, CommonCallDelegate handler)
        {
            ProtoCall call;
            if (!this.protos.TryGetValue(method, out call)) {
                this.protos.Add(method, new ProtoCall() {
                    Type = typeof(T),
                    Handler = handler,
                });
            } else {
                call.Handler = handler;
            }
        }

        public void Register<T>(string method, CallDelegate<T> handler)
        {
            ProtoCall call;
            if (!this.protos.TryGetValue(method, out call)) {
                this.protos.Add(method, new ProtoCall() {
                    Type = typeof(T),
                    Handler = handler,
                });
            } else {
                call.Handler = handler;
            }
        }

        public void Call<T>(string method, T content)
        {
            //if (null != this.server) {
            //    if (ServiceType.PULL == this.type) {
            //        Logger<IGiantLightServer>.L(string.Format("PullRequest {0}:{1}.", this.service, method));
            //        this.server.SendRequest(this.service, method, ByteConverter.ProtoBufToBytes(content));
            //    } else {
            //        Queue<uint> queue = null;
            //        if (this.calls.TryGetValue(method, out queue) && queue.Count > 0) {
            //            Logger<IGiantLightServer>.L(string.Format("PushResponse {0}:{1}.", this.service, method));
            //            this.server.SendResponse(queue.Dequeue(), ByteConverter.ProtoBufToBytes(content));
            //        }
            //    }
            //}
        }

        public bool PushRequest(uint id, string method, byte[] content)
        {
            ProtoCall call;
            if (this.protos.TryGetValue(method, out call) && null != call.Handler) {
                Logger<IGiantLightServer>.L(string.Format("PushRequest {0}:{1}.", this.service, method));
                Queue<uint> queue = null;
                if (this.calls.TryGetValue(method, out queue)) {
                    queue.Enqueue(id);
                } else {
                    queue = new Queue<uint>();
                    queue.Enqueue(id);
                    this.calls.Add(method, queue);
                }

                var request = ByteConverter.BytesToProtoBuf(call.Type, content, 0, content.Length);
                call.Handler.DynamicInvoke(method, request);

                return true;
            }

            Logger<IGiantLightServer>.W(string.Format("CANNOT find [{0}] handler for [{1}] service!", method, this.service));
            return false;
        }

        public bool PushResponse(string method, byte[] content)
        {
            ProtoCall call;
            if (this.protos.TryGetValue(method, out call)) {
                if (null != call.Handler) {
                    Logger<IGiantLightServer>.L(string.Format("PullResponse {0}:{1}.", this.service, method));
                    var response = ByteConverter.BytesToProtoBuf(call.Type, content, 0, content.Length);
                    call.Handler.DynamicInvoke(method, response);
                }

                return true;
            }

            Logger<IGiantLightServer>.W(string.Format("CANNOT find [{0}] handler for [{1}] service!", method, this.service));
            return false;
        }

        private struct ProtoCall
        {
            public Type Type;
            public Delegate Handler;
        }

        
        private string service = "";
        private ServiceType type = ServiceType.PULL;
        private Dictionary<string, ProtoCall> protos = new Dictionary<string, ProtoCall>();
        private Dictionary<string, Queue<uint>> calls = new Dictionary<string, Queue<uint>>();
    }
}