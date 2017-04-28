// -----------------------------------------------------------------
// File:    ClientInfoPack.cs
// Author:  mouguangyi
// Date:    2016.04.21
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.ClientCollector
{
    class ClientInfoPack : IClientInfoPack
    {
        public ClientInfoPack()
        {
            switch (Application.platform) {
            case RuntimePlatform.IPhonePlayer:
                this.bridge = new IOSClientNativeBridge();
                break;
            case RuntimePlatform.Android:
                this.bridge = new AndroidClientNativeBridge();
                break;
            default:
                this.bridge = new ClientNativeBridge();
                break;
            }

            DeviceIdentifier = this.bridge.GetDeviceIdentifier();
            if (string.IsNullOrEmpty(DeviceIdentifier)) {
                DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
            }

            Nation = this.bridge.GetCountryCode();
            Carrier = this.bridge.GetCarrierName();
            NetworkReachability = this.bridge.GetNetworkType();
            if (string.IsNullOrEmpty(NetworkReachability)) {
                switch (Application.internetReachability) {
                case UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork:
                    NetworkReachability = "Carrier";
                    break;
                case UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork:
                    NetworkReachability = "WLAN";
                    break;
                default:
                    NetworkReachability = "";
                    break;
                }
            }
        }

        public string DeviceMode
        {
            get {
                return SystemInfo.deviceModel;
            }
        }

        public string DeviceName
        {
            get {
                return SystemInfo.deviceName;
            }
        }

        public string DeviceIdentifier
        {
            get; private set;
        }

        public int GraphicsDeviceId
        {
            get {
                return SystemInfo.graphicsDeviceID;
            }
        }

        public string GraphicsDeviceName
        {
            get {
                return SystemInfo.graphicsDeviceName;
            }
        }

        public int GraphicsDeviceType
        {
            get {
                return (int)SystemInfo.graphicsDeviceType;
            }
        }

        public string GraphicsDeviceVendor
        {
            get {
                return SystemInfo.graphicsDeviceVendor;
            }
        }

        public string GraphicsDeviceVersion
        {
            get {
                return SystemInfo.graphicsDeviceVersion;
            }
        }

        public int GraphicsMemorySize
        {
            get {
                return SystemInfo.graphicsMemorySize;
            }
        }

        public string OperatingSystem
        {
            get {
                return SystemInfo.operatingSystem;
            }
        }

        public int ProcessorCount
        {
            get {
                return SystemInfo.processorCount;
            }
        }

        public int ProcessorFrequency
        {
            get {
                return SystemInfo.processorFrequency;
            }
        }

        public string ProcessorType
        {
            get {
                return SystemInfo.processorType;
            }
        }

        public bool DoesSupportGyroscope
        {
            get {
                return SystemInfo.supportsGyroscope;
            }
        }

        public bool DoesSupportLocationService
        {
            get {
                return SystemInfo.supportsLocationService;
            }
        }

        public int SystemMemorySize
        {
            get {
                return SystemInfo.systemMemorySize;
            }
        }

        public string NetworkReachability
        {
            get; private set;
        }

        public int ResolutionWidth
        {
            get {
                return Screen.currentResolution.width;
            }
        }

        public int ResolutionHeight
        {
            get {
                return Screen.currentResolution.height;
            }
        }

        public int ResolutionRefreshRate
        {
            get {
                return Screen.currentResolution.refreshRate;
            }
        }

        public string Nation
        {
            get; internal set;
        }

        public string Carrier
        {
            get; private set;
        }

        private ClientNativeBridge bridge = null;
    }
}
