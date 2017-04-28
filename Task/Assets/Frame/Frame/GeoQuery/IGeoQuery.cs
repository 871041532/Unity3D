// ---------------------------------------------------------------
// File: IGeoQueryEngine.cs
// Author: mouguangyi
// Date: 2016.03.25
// Description:
//   Interface for Geo query engine
// ---------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.GeoQuery
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGeoQuery : IService
    {
        /// <summary>
        /// 简单查询。
        /// </summary>
        /// <param name="ip">ip为空则查询本设备。</param>
        /// <returns>若成功，则Task.Result为IGeoResult。</returns>
        AsyncTask SimpleQuery(string ip = null);

        /// <summary>
        /// 详细查询。
        /// </summary>
        /// <param name="ip">ip为空则查询本机。</param>
        /// <returns>若成功，则Task.Result为IGeoResult。</returns>
        AsyncTask Query(string ip = null);
    }
}
