// -----------------------------------------------------------------
// File:    ISpriteAtlas.cs
// Author:  mouguangyi
// Date:    2016.06.07
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 图集接口。
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ISpriteAtlas
    {
        /// <summary>
        /// 读取对应的Sprite。
        /// </summary>
        /// <param name="name">Sprite名字。</param>
        /// <returns>对应的Sprite对象。</returns>
        Sprite GetSprite(string name);
    }
}