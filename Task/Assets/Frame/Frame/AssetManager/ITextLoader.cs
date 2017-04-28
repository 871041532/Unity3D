// -----------------------------------------------------------------
// File:    ITextLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 文本数据装载器。
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ITextLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法。
        /// </summary>
        /// <returns>返回读取的字符串。</returns>
        string Load();
    }
}