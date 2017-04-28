// -----------------------------------------------------------------
// File:    IAssetManagerUpdater.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetUpdater;
using System;

namespace GameBox.Service.AssetManagerUpdater
{
    /// <summary>
    /// @details 资产管理热更新服务接口。
    /// </summary>
    public interface IAssetManagerUpdater : IService
    {
        /// <summary>
        /// 应用强更新的安装包下载服务器地址。
        /// </summary>
        string AppServer { get; }

        /// <summary>
        /// 请求热更新服务器，获取更新状态和下载器。
        /// </summary>
        /// <param name="callback"></param>
        void Check(Action<AssetUpdateType, IAssetDownloader> callback);
    }
}