// -----------------------------------------------------------------
// File:    ClientCollectorInstaller.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.GeoQuery;
using UnityEngine;

namespace GameBox.Service.ClientCollector
{
    /// <summary>
    /// ClientCollectorInstaller是ClientCollector服务的安装器。
    /// </summary>
    [RequireComponent(typeof(ServiceCenter))]
    [RequireComponent(typeof(GeoQueryInstaller))]
    public sealed class ClientCollectorInstaller : ServiceInstaller<IClientCollector>
    {
        /// <summary>
        /// 创建服务。
        /// </summary>
        /// <returns>返回ClientCollector服务。</returns>
        protected override IService Create()
        {
            return new ClientCollector();
        }
    }
}
