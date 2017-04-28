// -----------------------------------------------------------------
// File:    AssetListUpdaterInstaller.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details 资产文件列表热更新服务加载器。
    /// 
    /// @li @c 对应的服务接口：IAssetListUpdater
    /// @li @c 对应的服务ID：com.giant.service.assetlistupdater
    /// 
    /// @see IAssetListUpdater
    ///  
    /// @section dependences 依赖
    /// 无。
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_asset_updater_1_1_i_asset_list_updater.html")]
    [DisallowMultipleComponent]
    public sealed class AssetListUpdaterInstaller : ServiceInstaller<IAssetListUpdater>
    {
        protected override IService Create()
        {
            return new AssetListUpdater();
        }
    }
}