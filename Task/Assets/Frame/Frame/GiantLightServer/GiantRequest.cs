//// -----------------------------------------------------------------
//// File:    GiantRequest.cs
//// Author:  mouguangyi
//// Date:    2016.06.16
//// Description:
////      
//// -----------------------------------------------------------------
//using GameBox.Service.ByteStorage;
//using rpc;
//using System;

//namespace GameBox.Service.GiantLightServer
//{
//    class GiantRequest : GiantPackage
//    {
//        public GiantRequest(RpcRequest request) : base(request.id)
//        {
//            this.request = request;
//        }

//        public override void Dispose()
//        {
//            this.request = null;

//            base.Dispose();
//        }

//        public IByteArray ToByteArray(IByteStorage storage)
//        {
//            if (null == this.bytes) {
//                var pkg = new RpcPackage();
//                pkg.request = this.request;

//                var bytes = ByteConverter.ProtoBufToBytes(pkg);
//                this.bytes = storage.Alloc(sizeof(UInt32) + bytes.Length);
//                this.bytes.WriteUInt32((UInt32)bytes.Length);
//                this.bytes.WriteBytes(bytes, 0, bytes.Length);
//            }

//            return this.bytes;
//        }

//        public string Service
//        {
//            get {
//                return this.request.service;
//            }
//        }

//        public string Method
//        {
//            get {
//                return this.request.method;
//            }
//        }

//        public byte[] Content
//        {
//            get {
//                return this.request.content;
//            }
//        }

//        private RpcRequest request = null;
//    }
//}