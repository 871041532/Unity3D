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
    /// @details Spriteװ������
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ISpriteAtlasLoader : IAssetLoader
    {
        /// <summary>
        /// ͬ��װ�ط�����
        /// </summary>
        /// <returns>ISpriteAtlas�ӿڶ���<see cref="ISpriteAtlas"/></returns>
        ISpriteAtlas Load();
    }
}