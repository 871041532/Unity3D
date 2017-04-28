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
    /// @details ����װ������
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ITextureLoader : IAssetLoader
    {
        /// <summary>
        /// ͬ��װ�ط�����
        /// </summary>
        /// <returns>���ض�ȡ������</returns>
        Texture Load();
    }
}