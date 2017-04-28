// -----------------------------------------------------------------
// File:    ServiceBaseInstaller.cs
// Author:  mouguangyi
// Date:    2017.01.10
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Framework
{
    public abstract class ServiceBaseInstaller : MonoBehaviour
    {
        internal virtual void _Install(ServiceCenter center)
        { }
    }
}