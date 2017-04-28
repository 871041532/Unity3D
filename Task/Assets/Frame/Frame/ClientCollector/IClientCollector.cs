// -----------------------------------------------------------------
// File:    IClientCollector.cs
// Author:  mouguangyi
// Date:    2016.04.21
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.ClientCollector
{
    /// <summary>
    /// IClientCollector是客户端数据搜集的对外服务接口。
    /// </summary>
    public interface IClientCollector : IService
    {
        /// <summary>
        /// 搜集客户端基础数据信息。
        /// </summary>
        /// <returns>返回IClientInfoPack。</returns>
        IClientInfoPack Collect();
    }
}
