// -----------------------------------------------------------------
// File:    IBytesLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details ����������װ������
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IBytesLoader : IAssetLoader
    {
        /// <summary>
        /// ͬ��װ�ط�����
        /// </summary>
        /// <returns>��ȡ��byte���顣</returns>
        byte[] Load();
    }
}