// -----------------------------------------------------------------
// File:    IAssetDownloader.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details 资产热更新下载器。
    /// </summary>
    public interface IAssetDownloader
    {
        /// <summary>
        /// 资产版本。
        /// </summary>
        string AssetVersion { get; }

        /// <summary>
        /// 总共需要下载大小。
        /// </summary>
        long TotalSize { get; }

        /// <summary>
        /// 已经下载的大小。
        /// </summary>
        long DownloadedSize { get; }

        /// <summary>
        /// 启动下载。
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="parallelCount">同时下载文件的个数。默认值为-1，表示所有资源同时下载。</param>
        void Start(Action callback, int parallelCount = -1);
    }
}