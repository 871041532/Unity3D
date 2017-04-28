// -----------------------------------------------------------------
// File:    IOSClientNativeBridge.cs
// Author:  mouguangyi
// Date:    2016.04.25
// Description:
//      
// -----------------------------------------------------------------
using System.Runtime.InteropServices;

namespace GameBox.Service.ClientCollector
{
    class IOSClientNativeBridge : ClientNativeBridge
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern string _IOSNativeCollector_GetDeviceIdentifier();

        [DllImport("__Internal")]
        private static extern string _IOSNativeCollector_GetNetworkType();

        [DllImport("__Internal")]
        private static extern string _IOSNativeCollector_GetCountryCode();

        [DllImport("__Internal")]
        private static extern string _IOSNativeCollector_GetCarrierName();

        public override string GetDeviceIdentifier()
        {
            return _IOSNativeCollector_GetDeviceIdentifier();
        }

        public override string GetNetworkType()
        {
            return _IOSNativeCollector_GetNetworkType();
        }

        public override string GetCountryCode()
        {
            return _IOSNativeCollector_GetCountryCode();
        }

        public override string GetCarrierName()
        {
            return _IOSNativeCollector_GetCarrierName();
        }
#endif
    }
}
