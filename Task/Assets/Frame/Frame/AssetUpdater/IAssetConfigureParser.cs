// -----------------------------------------------------------------
// File:    IAssetConfigureParser.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details 资产配置文件解析器，需要接入端实现。
    /// </summary>
    public interface IAssetConfigureParser
    {
        /// <summary>
        /// 异步解析。
        /// </summary>
        /// <param name="data">数据流。</param>
        /// <param name="handler">解析结束响应句柄。</param>
        /// <returns></returns>
        void ParseAsync(byte[] data, Action<IAssetConfigure> handler);
    }
}