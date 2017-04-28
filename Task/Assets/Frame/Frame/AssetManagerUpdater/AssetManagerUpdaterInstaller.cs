// -----------------------------------------------------------------
// File:    AssetManagerUpdaterInstaller.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using GameBox.Service.AssetUpdater;
using UnityEngine;

namespace GameBox.Service.AssetManagerUpdater
{
    /// <summary>
    /// @details 资产管理热更新服务。
    /// 
    /// @li @c 对应的服务接口：IAssetManagerUpdater
    /// @li @c 对应的服务ID：com.giant.service.assetmanagerupdater
    /// 
    /// @see IAssetManagerUpdater
    /// 
    /// @section dependences 依赖
    /// @li @c AssetManagerInstaller
    /// @li @c AssetListUpdaterInstaller
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_asset_manager_updater_1_1_i_asset_manager_updater.html")]
    [RequireComponent(typeof(AssetManagerInstaller))]
    [RequireComponent(typeof(AssetListUpdaterInstaller))]
    [DisallowMultipleComponent]
    public sealed class AssetManagerUpdaterInstaller : ServiceInstaller<IAssetManagerUpdater>
    {
        /// <summary>
        /// 应用强更新的安装包下载服务器地址。
        /// </summary>
        public string AppServer;

        /// <summary>
        /// 应用的热更新资源服务器地址。
        /// </summary>
        public string AssetServer;

        /// <summary>
        /// 功能是否有效。
        /// 
        /// @note 在开发阶段设置为false可以避免开发中触发热更新流程。
        /// </summary>
        public bool Valid = true;

        protected override IService Create()
        {
            return new AssetManagerUpdater();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("AppServer", this.AppServer);
            args.Set("AssetServer", this.AssetServer);
            args.Set("Valid", this.Valid);
        }
    }
}