//// -----------------------------------------------------------------
//// File:    GiantLightServer.cs
//// Author:  mouguangyi
//// Date:    2016.06.16
//// Description:
////      
//// -----------------------------------------------------------------
//using GameBox.Framework;
//using GameBox.Service.ByteStorage;
//using GameBox.Service.NetworkManager;
////using rpc;
//using System.Collections.Generic;
//using System.Net;
//using UnityEngine;

//namespace GameBox.Service.GiantLightServer
//{
//    sealed class GiantLightServer : IGiantLightServer, IServiceGraph
//    {
//        #region IService
//        public string Id
//        {
//            get {
//                return "com.giant.service.giantlightserver";
//            }
//        }

//        public void Run(IServiceRunner runner)
//        {
//            new ServicesTask(new string[] {
//                "com.giant.service.bytestorage",
//                "com.giant.service.networkmanager",
//            }).Start().Continue(task =>
//            {
//                var services = task.Result as IService[];
//                this.storage = services[0] as IByteStorage;
//                var network = services[1] as INetworkManager;
//                this.channel = network.Create("tcp") as ISocketChannel;
//                runner.Ready(_Terminate);
//                return null;
//            });
//        }

//        public void Pulse(float delta)
//        {
//            var node = this.calls.First;
//            while (null != node) {
//                var current = node;
//                node = node.Next;

//                var call = current.Value;
//                switch (call.State) {
//                case GiantCallState.NOTSENT:
//                    if (GiantCallType.S2C == call.Type) {
//                        GiantLightProxy proxy = null;
//                        if ((this.proxies.TryGetValue(call.Request.Service, out proxy)                     &&
//                            proxy.PushRequest(call.Request.Id, call.Request.Method, call.Request.Content))  ||
//                            this.client.PushRequest(call.Request.Id, call.Request.Service, call.Request.Method, call.Request.Content)) {
//                            call.State = GiantCallState.NOTRECEIVED;
//                        }
//                    }
//                    break;
//                case GiantCallState.NOTRECEIVED:
//                    if (null != call.Response && GiantCallType.C2S == call.Type) {
//                        GiantLightProxy proxy = null;
//                        if ((this.proxies.TryGetValue(call.Request.Service, out proxy)     &&
//                            proxy.PushResponse(call.Request.Method, call.Response.Content)) ||
//                            this.client.PushResponse(call.Request.Service, call.Request.Method, call.Response.Content)) {
//                            _RemoveCall(call);
//                        }
//                    }
//                    break;
//                }
//            }

//            //while (this.channel.Receive(bytes =>
//            {
//                _BeginMessage(bytes);
//                _ProcessMessage();
//                _EndMessage();
//            })) { }
//        }
//        #endregion

//        #region IGiantLightServer
//        public void Connect(string ip, int port, IGiantLightClient client)
//        {
//            if (null == client) {
//                Logger<IGiantLightServer>.E("IGiantLightClient can't be null!");
//            }

//            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
//            this.client = client;
//            this.channel.OnChannelStateChange = _OnChannelStateChange;
//            this.channel.Connect(this.ipEndPoint);
//        }

//        public void Disconnect()
//        {
//            _OnDisconnect();

//            this.channel.Disconnect();
//        }

//        public void SendRequest(string service, string method, byte[] content)
//        {
//            //if (this.connected) {
//            //    var request = new RpcRequest();
//            //    request.id = RequestAllocator.NextId();
//            //    request.service = service;
//            //    request.method = method;
//            //    request.content = content;

//            //    var call = _AddRequest(GiantCallType.C2S, new GiantRequest(request));
//            //    call.State = GiantCallState.NOTRECEIVED;
//            //    this.channel.Send(call.Request.ToByteArray(this.storage));
//            }
//        }

//        public void SendResponse(uint id, byte[] content)
//        {
//            if (this.connected) {
//                var response = new RpcResponse();
//                response.id = id;
//                response.content = content;

//                var call = _AddResponse(GiantCallType.S2C, new GiantResponse(response));
//                this.channel.Send(call.Response.ToByteArray(this.storage));
//                _RemoveCall(call);
//            }
//        }

//        public IGiantLightProxy CreateProxy(string service, ServiceType type)
//        {
//            GiantLightProxy proxy = null;
//            if (!this.proxies.TryGetValue(service, out proxy)) {
//                proxy = new GiantLightProxy(this, service, type);
//                this.proxies.Add(service, proxy);
//            }

//            return proxy;
//        }

//        public IGiantLightProxy FindProxy(string service)
//        {
//            GiantLightProxy proxy = null;
//            if (this.proxies.TryGetValue(service, out proxy)) {
//                return proxy;
//            } else {
//                return null;
//            }
//        }

//        public bool Connected
//        {
//            get {
//                return this.connected;
//            }
//        }
//        #endregion

//        internal void _DeleteProxy(string service)
//        {
//            this.proxies.Remove(service);
//        }

//        private void _Terminate()
//        {
//            _ClearMessage();

//            if (null != this.channel) {
//                this.channel.Dispose();
//                this.channel = null;
//            }

//            if (null != this.proxies) {
//                this.proxies.Clear();
//                this.proxies = null;
//            }

//            this.calls = null;
//            this.table = null;
//            this.storage = null;
//        }

//        private void _OnChannelStateChange(string state)
//        {
//            switch (state) {
//            case ChannelState.CONNECTED:
//                this.connected = true;
//                this.client.OnConnect();
//                break;
//            case ChannelState.DISCONNECTED:
//                _OnDisconnect();
//                this.client.OnDisconnect();
//                break;
//            }
//        }

//        private void _OnDisconnect()
//        {
//            _ClearMessage();

//            var node = this.calls.First;
//            while (null != node) {
//                node.Value.Dispose();
//                node = node.Next;
//            }

//            this.calls.Clear();
//            this.table.Clear();
//            this.connected = false;
//        }

//        private void _BeginMessage(IByteArray bytes)
//        {
//            if (null == this.message) {
//                this.message = bytes;
//                this.message.Retain();
//            } else {
//                var combineBytes = this.storage.Alloc(this.message.Size - this.message.Position + bytes.Size);
//                combineBytes.WriteByteArray(this.message);
//                combineBytes.WriteByteArray(bytes);
//                combineBytes.Seek();

//                this.message.Release();
//                this.message = combineBytes;
//            }
//        }

//        private void _EndMessage()
//        {
//            if (this.message.Position == this.message.Size) {
//                this.message.Release();
//                this.message = null;
//            }
//        }

//        private void _ProcessMessage()
//        {
//            while (this.message.Position < this.message.Size && (this.message.Size - this.message.Position) > 4) {
//                var length = this.message.ReadUInt32();
//                if (this.message.Position + length > this.message.Size) {
//                    this.message.Seek(-4, SeekOrigin.CURRENT);
//                    break;
//                }

//                //v/ar pkg = ByteConverter.ByteArrayToProtoBuf<RpcPackage>(this.message, this.message.Position, (int)length);
//                //if (null != pkg) {
//                //    if (null != pkg.request) {
//                //        _AddRequest(GiantCallType.S2C, new GiantRequest(pkg.request));
//                //    } else if (null != pkg.response) {
//                //        _AddResponse(GiantCallType.C2S, new GiantResponse(pkg.response));
//                //    } else {
//                //        Logger<IGiantLightServer>.E("Incorrect RpcPackage result!");
//                //    }
//                //} else {
//                //    Logger<IGiantLightServer>.E("Unpack RpcPackage failed!");
//                //}
//                //this.message.Seek((int)length, SeekOrigin.CURRENT);
//            }
//        }

//        private void _ClearMessage()
//        {
//            if (null != this.message) {
//                this.message.Release();
//                this.message = null;
//            }
//        }

//        private GiantCall _AddRequest(GiantCallType type, GiantRequest request)
//        {
//            var call = new GiantCall(type, request);
//            this.calls.AddLast(call);
//            this.table.Add(_GetTableId(type, request.Id), call);

//            return call;
//        }

//        private GiantCall _AddResponse(GiantCallType type, GiantResponse response)
//        {
//            GiantCall call = null;
//            if (table.TryGetValue(_GetTableId(type, response.Id), out call)) {
//                call.Response = response;
//            }

//            return call;
//        }

//        private void _RemoveCall(GiantCall call)
//        {
//            lock (this.calls) {
//                this.calls.Remove(call);
//            }
//            lock (this.table) {
//                this.table.Remove(_GetTableId(call.Type, call.Request.Id));
//            }
//            call.Dispose();
//        }

//        private string _GetTableId(GiantCallType type, uint id)
//        {
//            return (type == GiantCallType.C2S ? "c" : "s") + id;
//        }

//        private static class RequestAllocator
//        {
//            public static uint NextId()
//            {
//                if (uint.MaxValue == (++primaryId)) {
//                    primaryId = 0;
//                }

//                return primaryId;
//            }

//            private static uint primaryId = 0;
//        }

//        private IByteStorage storage = null;
//        private bool connected = false;
//        private IPEndPoint ipEndPoint = null;
//        private ISocketChannel channel = null;
//        private IGiantLightClient client = null;
//        private IByteArray message = null;
//        private LinkedList<GiantCall> calls = new LinkedList<GiantCall>();
//        private Dictionary<string, GiantCall> table = new Dictionary<string, GiantCall>();
//        private Dictionary<string, GiantLightProxy> proxies = new Dictionary<string, GiantLightProxy>();

//        // ---------------------------------------------------------
//        // Graph
//        public void Draw()
//        {
//            var center = GraphStyle.ServiceWidth * 0.5f;
//            GUI.Label(new Rect(0, 0, Width, CALL_LINE_HEIGHT), "Calls: " + this.calls.Count, GraphStyle.LightGrayBox);
//            GUI.DrawTexture(new Rect(center - 1f, CALL_LINE_HEIGHT, 2, Height - CALL_LINE_HEIGHT), GraphStyle.LightGrayTexture);
//            int yOffset = CALL_LINE_HEIGHT;
//            var node = this.calls.First;
//            while (null != node) {
//                var call = node.Value;
//                switch (call.State) {
//                case GiantCallState.NOTSENT:
//                    if (GiantCallType.C2S == call.Type) {
//                        GUI.DrawTexture(new Rect(center - 1f - CALL_LINE_WIDTH, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), GraphStyle.RedTexture);
//                        GUI.Label(new Rect(center - 1f - CALL_LINE_WIDTH, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), call.Request.Method, GraphStyle.SmallLabel);
//                    } else if (GiantCallType.S2C == call.Type) {
//                        GUI.DrawTexture(new Rect(center + 1f, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), GraphStyle.RedTexture);
//                        GUI.Label(new Rect(center + 1f, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), call.Request.Method, GraphStyle.SmallLabel);
//                    }
//                    break;
//                case GiantCallState.NOTRECEIVED:
//                    if (GiantCallType.C2S == call.Type) {
//                        GUI.DrawTexture(new Rect(center - 1f - CALL_LINE_WIDTH, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), GraphStyle.GreenTexture);
//                        GUI.Label(new Rect(center - 1f - CALL_LINE_WIDTH, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), call.Request.Method, GraphStyle.SmallLabel);
//                    } else if (GiantCallType.S2C == call.Type) {
//                        GUI.DrawTexture(new Rect(center + 1f, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), GraphStyle.GreenTexture);
//                        GUI.Label(new Rect(center + 1f, yOffset, CALL_LINE_WIDTH, CALL_LINE_HEIGHT), call.Request.Method, GraphStyle.SmallLabel);
//                    }
//                    break;
//                }
//                yOffset += CALL_LINE_HEIGHT + CALL_LINE_MARGIN;
//                node = node.Next;
//            }
//        }

//        public float Width
//        {
//            get {
//                return GraphStyle.ServiceWidth;
//            }
//        }

//        public float Height
//        {
//            get {
//                return Mathf.Max(20 + this.calls.Count * (CALL_LINE_HEIGHT + 2), 100f);
//            }
//        }

//        private const int CALL_LINE_WIDTH = 100;
//        private const int CALL_LINE_HEIGHT = 30;
//        private const int CALL_LINE_MARGIN = 1;
//    }
//}