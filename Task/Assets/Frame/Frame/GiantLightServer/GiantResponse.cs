//// -----------------------------------------------------------------
//// File:    GiantResponse.cs
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
//    class GiantResponse : GiantPackage
//    {
//        public GiantResponse(RpcResponse response) : base(response.id)
//        {
//            this.response = response;
//        }

//        public override void Dispose()
//        {
//            this.response = null;

//            base.Dispose();
//        }

//        public IByteArray ToByteArray(IByteStorage storage)
//        {
//            if (null == this.bytes) {
//                var pkg = new RpcPackage();
//                pkg.response = this.response;

//                var bytes = ByteConverter.ProtoBufToBytes<RpcPackage>(pkg);
//                this.bytes = storage.Alloc(sizeof(UInt32) + bytes.Length);
//                this.bytes.WriteUInt32((UInt32)bytes.Length);
//                this.bytes.WriteBytes(bytes, 0, bytes.Length);
//            }

//            return this.bytes;
//        }

//        public byte[] Content
//        {
//            get {
//                return this.response.content;
//            }
//        }

//        private RpcResponse response = null;
//    }
//}