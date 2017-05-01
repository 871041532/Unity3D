// -----------------------------------------------------------------
// File:    IAssetManager.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using UnityEngine.SceneManagement;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 资产管理服务是GameBox平台提供的方便易用的资源装载和读取服务。通过获取到IAssetManager接口，使用者可以根据资源的相对路径和资源类型创建对应的装载器。装载器提供两种资产装载方式：同步和异步。使用者根据使用场景决定是要同步加载还是异步加载。资产管理服务屏蔽了资源的存储位置，使用者无须关心资源是存储在Resources下，还是StreamingAssets下，亦或是PersistentDataPath下。同时资产管理服务也屏蔽了资源的存储方式，使用者也无须关心资源是以打包的方式存在一个AssetBundle中，亦或是以原始状态独立存储。
    /// 
    /// @note 因为资产管理服务依赖于自身提供的一个资源配置文件，因此资产管理服务在初始化时会加载这个文件。但是如果此时发生资源更新，有新的资源覆盖本地，那么需要使用者在资源更新完成后主动触发UpdateAsync方法，以确保运行时使用最新的资源。
    /// 
    /// @section example 例子
    /// @code{.cs}
    /// // 根据资产管理服务的ID异步获取资产管理服务
    /// new ServiceTask("com.giant.service.assetmanager").Start().Continue(task =>
    /// {
    ///   var assetManager = task.Result as IAssetManager;
    ///   return null;
    /// });
    /// @endcode
    /// 
    /// @code{.cs}
    /// // 根据资产管理服务接口异步获取资产管理服务
    /// new ServiceTask<IAssetManager>().Start().Continue(task =>
    /// {
    ///   var assetManager = task.Result as IAssetManager;
    ///   return null;
    /// });
    /// @endcode
    /// 
    /// @code {.cs}
    /// // 如果能确保资产管理服务已经运行，可以直接获取
    /// var assetManager = TaskCenter.GetService<IAssetManager>();
    /// @endcode
    /// </summary>
    public interface IAssetManager : IService
    {
        /// <summary>
        /// 资产版本。
        /// </summary>
        string AssetVersion { get; }

        /// <summary>
        /// 根据资源路径和类型，同步装载资源。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IAsset Load(string path, AssetType type);

        /// <summary>
        /// 根据资源路径和类型，异步装载资源。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        void LoadAsync(string path, AssetType type, Action<IAsset> handler);

        /// <summary>
        /// 根据路径和模式，同步装载场景。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IAsset LoadScene(string path, LoadSceneMode mode);

        /// <summary>
        /// 根据路径和模式，异步装载场景。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="handler"></param>
        void LoadSceneAsync(string path, LoadSceneMode mode, Action<IAsset> handler);

        /// <summary>
        /// 根据路径和类型创建装载器。
        /// @note 资源路径要带上文件扩展名，例如.prefab，.txt等等。
        /// </summary>
        /// <param name="path">资源路径。</param>
        /// <param name="type">资源类型。</param>
        /// <returns>返回对应的资源加载器。</returns>
        [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
        IAssetLoader CreateLoader(string path, AssetType type);

        /// <summary>
        /// 根据路径和装载器类型创建装载器。
        /// @note 资源路径要带上文件扩展名，例如.prefab，.txt等等。
        /// </summary>
        /// <typeparam name="T">资源加载器类型。</typeparam>
        /// <param name="path">资源路径。</param>
        /// <returns>返回对应的资源加载器。</returns>
        [Obsolete("Use IAssetManager.Load/IAssetManager.LoadAsync directly!")]
        T CreateLoader<T>(string path) where T : IAssetLoader;

        /// <summary>
        /// 无效资产立即回收。
        /// </summary>
        void GC();

        /// <summary>
        /// 更新运行时资源配置表。资源配置表在AssetManager服务启动时会自动更新一次，但如果之后有资源热更新，那么在更新后也应该主动调用一次以确保和最新的资源一致。
        /// </summary>
        /// <param name="callback">更新完成的回调函数句柄。</param>
        void UpdateAsync(Action callback);
    }
}