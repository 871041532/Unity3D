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
    /// @details 资产装载器基础接口。
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IAssetLoader : IDisposable
    {
        /// <summary>
        /// 装载资源的异步方法。
        /// </summary>
        /// <param name="handler">异步回调函数句柄。</param>
        void LoadAsync(Action<object> handler);
    }
}