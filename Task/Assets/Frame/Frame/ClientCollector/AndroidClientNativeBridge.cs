// -----------------------------------------------------------------
// File:    AndroidClientNativeBridge.cs
// Author:  mouguangyi
// Date:    2016.04.25
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.ClientCollector
{
    class AndroidClientNativeBridge : ClientNativeBridge
    {
#if UNITY_ANDROID
        public const string ANDROIDCLASS_NAME = "com.ztgame.nativebridge.AndroidNativeCollector";
        public const string GETNETWORKTYPE_NAME = "getNetworkType";
        public const string GETCOUNTRYCODE_NAME = "getCountryCode";
        public const string GETCARRIERNAME_NAME = "getCarrierName";

        public AndroidClientNativeBridge()
        {
            androidClass = new AndroidJavaClass(ANDROIDCLASS_NAME);
        }

        public override string GetNetworkType()
        {
            return androidClass.CallStatic<string>(GETNETWORKTYPE_NAME, new object[] { });    
        }

        public override string GetCountryCode()
        {
            return androidClass.CallStatic<string>(GETCOUNTRYCODE_NAME, new object[] { });
        }

        public override string GetCarrierName()
        {
            return androidClass.CallStatic<string>(GETCARRIERNAME_NAME, new object[] { });
        }

        private AndroidJavaClass androidClass = null;
#endif
    }
}
