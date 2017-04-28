// ---------------------------------------------------------------
// File: GeoQuery.cs
// Author: mouguangyi
// Date: 2016.03.25
// Description:
//   Static class for Geo query utility
// ---------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.GeoQuery
{
    /// <summary>
    /// GeoQuery服务的安装器。
    /// </summary>
    [RequireComponent(typeof(ServiceCenter))]
    public sealed class GeoQueryInstaller : ServiceInstaller<IGeoQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GeoEngineType QueryEngineType = GeoEngineType.SHINY;
        /// <summary>
        /// 
        /// </summary>
        /// <returns>GeoQuery服务</returns>
        protected override IService Create()
        {
            var service = new GeoQuery();
            service.QueryEngineType = this.QueryEngineType;

            return service;
        }
    }
}
