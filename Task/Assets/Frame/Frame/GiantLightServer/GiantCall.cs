//// -----------------------------------------------------------------
//// File:    GiantCall.cs
//// Author:  mouguangyi
//// Date:    2016.06.16
//// Description:
////      
//// -----------------------------------------------------------------
//using System;

//namespace GameBox.Service.GiantLightServer
//{
//    enum GiantCallType
//    {
//        C2S,
//        S2C,
//    }

//    enum GiantCallState
//    {
//        NOTSENT,
//        NOTRECEIVED,
//    }

//    sealed class GiantCall : IDisposable
//    {
//        public GiantCall(GiantCallType type, GiantRequest request)
//        {
//            this.type = type;
//            this.request = request;
//        }

//        public void Dispose()
//        {
//            if (null != this.request) {
//                this.request.Dispose();
//                this.request = null;
//            }
//            if (null != this.response) {
//                this.response.Dispose();
//                this.response = null;
//            }
//        }

//        public GiantCallType Type
//        {
//            get {
//                return this.type;
//            }
//        }

//        public GiantCallState State
//        {
//            get {
//                return this.state;
//            }
//            set {
//                this.state = value;
//            }
//        }

//        public GiantRequest Request
//        {
//            get {
//                return this.request;
//            }
//        }

//        public GiantResponse Response
//        {
//            get {
//                return this.response;
//            }
//            set {
//                this.response = value;
//            }
//        }

//        private GiantCallType type = GiantCallType.C2S;
//        private GiantCallState state = GiantCallState.NOTSENT;
//        private GiantRequest request = null;
//        private GiantResponse response = null;
//    }
//}