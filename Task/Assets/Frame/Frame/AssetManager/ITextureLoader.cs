// -----------------------------------------------------------------
// File:    ITextureLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 纹理装载器。
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ITextureLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法。
        /// </summary>
        /// <returns>返回读取的纹理。</returns>
        Texture Load();
    }
}