// -----------------------------------------------------------------
// File:    IBytesLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 二进制数据装载器。
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IBytesLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法。
        /// </summary>
        /// <returns>读取的byte数组。</returns>
        byte[] Load();
    }
}