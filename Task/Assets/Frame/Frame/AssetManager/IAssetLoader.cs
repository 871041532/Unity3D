// -----------------------------------------------------------------
// File:    IAssetLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details �ʲ�װ���������ӿڡ�
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IAssetLoader : IDisposable
    {
        /// <summary>
        /// װ����Դ���첽������
        /// </summary>
        /// <param name="handler">�첽�ص����������</param>
        void LoadAsync(Action<object> handler);
    }
}