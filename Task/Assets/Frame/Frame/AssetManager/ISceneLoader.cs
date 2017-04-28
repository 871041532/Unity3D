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
    /// @details 场景装载器。
    /// </summary>
    [Obsolete("Use IAssetManager.LoadScene/IAssetManager.LoadSceneAsync directly!")]
    public interface ISceneLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法。
        /// </summary>
        void Load();

        /// <summary>
        /// 增量同步装载方法。
        /// </summary>
        void LoadAdditive();

        /// <summary>
        /// 增量异步加载方法。
        /// </summary>
        /// <param name="handler"></param>
        void LoadAdditiveAsync(Action<object> handler);
    }
}