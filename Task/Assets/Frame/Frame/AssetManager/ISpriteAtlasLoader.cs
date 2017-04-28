// -----------------------------------------------------------------
// File:    ISpriteAtlasLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details Sprite装载器。
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ISpriteAtlasLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法。
        /// </summary>
        /// <returns>ISpriteAtlas接口对象。<see cref="ISpriteAtlas"/></returns>
        ISpriteAtlas Load();
    }
}