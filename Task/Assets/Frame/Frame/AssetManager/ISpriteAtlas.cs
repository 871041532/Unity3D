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
    /// @details ͼ���ӿڡ�
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ISpriteAtlas
    {
        /// <summary>
        /// ��ȡ��Ӧ��Sprite��
        /// </summary>
        /// <param name="name">Sprite���֡�</param>
        /// <returns>��Ӧ��Sprite����</returns>
        Sprite GetSprite(string name);
    }
}