// -----------------------------------------------------------------
// File:    UISystemInstaller.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using GameBox.Service.ObjectPool;
using UnityEngine;

namespace GameBox.Service.UI
{
    /// <summary>
    /// @details UI系统服务安装器。
    /// 
    /// @li @c 对应的服务接口：IUISystem
    /// @li @c 对应的服务ID：com.giant.service.uisystem
    /// 
    /// @see IUISystem
    /// 
    /// @section dependencies 依赖
    /// @li @c AssetManagerInstaller
    /// @li @c ObjectPoolInstaller
    /// 
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_u_i_1_1_i_u_i_system.html")]
    [RequireComponent(typeof(AssetManagerInstaller))]
    [RequireComponent(typeof(RecycleManagerInstaller))]
    public sealed class UISystemInstaller : ServiceInstaller<IUISystem>
    {
        protected override IService Create()
        {
            return new UISystem();
        }
    }
}
