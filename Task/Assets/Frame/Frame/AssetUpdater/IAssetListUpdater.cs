// -----------------------------------------------------------------
// File:    IAssetListUpdater.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details 资产文件列表更新服务接口。
    /// </summary>
    public interface IAssetListUpdater : IService
    {
        /// <summary>
        /// 请求服务器指定文件，经解析处理并返回热更新状态。
        /// </summary>
        /// <param name="updateServerPath">更新服务地址</param>
        /// <param name="configureFile">配置文件名</param>
        /// <param name="parser">接入端需要实现的资产配置文件解析器</param>
        /// <param name="callback"></param>
        void Check(string updateServerPath, string configureFile, IAssetConfigureParser parser, Action<AssetUpdateType, IAssetDownloader> callback);
    }
}


