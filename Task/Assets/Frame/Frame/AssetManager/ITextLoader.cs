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
    /// @details �ı�����װ������
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface ITextLoader : IAssetLoader
    {
        /// <summary>
        /// ͬ��װ�ط�����
        /// </summary>
        /// <returns>���ض�ȡ���ַ�����</returns>
        string Load();
    }
}