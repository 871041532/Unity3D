// -----------------------------------------------------------------
// File:    ClientNativeBridge.cs
// Author:  mouguangyi
// Date:    2016.04.25
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.ClientCollector
{
    class ClientNativeBridge
    {
        public virtual string GetDeviceIdentifier()
        { return ""; }

        public virtual string GetNetworkType()
        { return ""; }

        public virtual string GetCountryCode()
        { return ""; }

        public virtual string GetCarrierName()
        { return ""; }
    }
}
