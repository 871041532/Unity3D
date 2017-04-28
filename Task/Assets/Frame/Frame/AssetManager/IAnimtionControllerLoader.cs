// -----------------------------------------------------------------
// File:    IAnimtionControllerLoader.cs
// Author:  fuzhun
// Date:    2016.11.17
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 动画控制器加载器
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IAnimtionControllerLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法
        /// </summary>
        /// <returns>返回读取的纹理控制器</returns>
        RuntimeAnimatorController Load();
    }
}
