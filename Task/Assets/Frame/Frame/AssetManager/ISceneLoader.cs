// -----------------------------------------------------------------
// File:    ISceneLoader.cs
// Author:  mouguangyi
// Date:    2016.07.11
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details ����װ������
    /// </summary>
    [Obsolete("Use IAssetManager.LoadScene/IAssetManager.LoadSceneAsync directly!")]
    public interface ISceneLoader : IAssetLoader
    {
        /// <summary>
        /// ͬ��װ�ط�����
        /// </summary>
        void Load();

        /// <summary>
        /// ����ͬ��װ�ط�����
        /// </summary>
        void LoadAdditive();

        /// <summary>
        /// �����첽���ط�����
        /// </summary>
        /// <param name="handler"></param>
        void LoadAdditiveAsync(Action<object> handler);
    }
}