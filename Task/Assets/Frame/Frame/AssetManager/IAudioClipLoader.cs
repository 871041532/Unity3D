// -----------------------------------------------------------------
// File:    IAudioClipLoader.cs
// Author:  fuzhun
// Date:    2016.09.18
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 声音装载器。
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IAudioClipLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法。
        /// </summary>
        /// <returns>返回读取的声音资源。</returns>
        AudioClip Load();
    }
}