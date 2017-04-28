// -----------------------------------------------------------------
// File:    AssetManagerInstaller.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 资产管理服务安装器。
    /// 
    /// @li @c 对应的服务接口：IAssetManager
    /// @li @c 对应的服务ID：com.giant.service.assetmanager
    /// 
    /// @see IAssetManager
    /// 
    /// @section dependencies 依赖
    /// 无。
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_asset_manager_1_1_i_asset_manager.html")]
    [DisallowMultipleComponent]
    public sealed class AssetManagerInstaller : ServiceInstaller<IAssetManager>
    {
        protected override IService Create()
        {
            return new AssetManager();
        }
    }
}