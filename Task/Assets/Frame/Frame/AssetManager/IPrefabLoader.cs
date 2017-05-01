// -----------------------------------------------------------------
// File:    IPrefabLoader.cs
// Author:  lvguanghai
// Date:    2016.06.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details Prefab装载器。
    /// 
    /// @note Prefab加载器并不会维护使用他装载的资源实例化的GameObject的生命周期，使用者创建的GameObject需要在不需要时自己销毁或者用ObjectPool循环使用。
    /// 
    /// @section example 例子
    /// @code{.cs}
    /// var assetManager = TaskCenter.GetService<IAssetManager>();
    /// var loader = assetManager.CreateLoader<IPrefabLoader>("XXX.prefab");
    /// var asset = loader.Load(); // 同步装载
    /// var go = GameObject.Instantiate(asset) as GameObject;
    /// ...
    /// // 销毁
    /// loader.Dispose();
    /// GameObject.Destroy(go);
    /// @endcode
    /// </summary>
    [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
    public interface IPrefabLoader : IAssetLoader
    {
        /// <summary>
        /// 同步装载方法
        /// </summary>
        /// <returns>成功则返回UnityEngine.Object；否则返回null。</returns>
        UnityEngine.Object Load();
    }
}