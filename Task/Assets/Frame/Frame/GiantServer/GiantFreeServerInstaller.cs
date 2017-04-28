// -----------------------------------------------------------------
// File:    GiantFreeServerInstaller.cs
// Author:  fuzhun
// Date:    2016.08.05
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NetworkManager;
using UnityEngine;

namespace GameBox.Service.GiantFreeServer
{
    /// <summary>
    /// @details 巨人端游服务器接入服务装载器。
    /// 
    /// @li @c 对应的服务接口：IGiantFreeServer
    /// @li @c 对应的服务ID：com.giant.service.giantfreeserver
    /// 
    /// @see IGiantFreeServer
    ///  
    /// @section dependences 依赖
    /// @li @c NetworkManagerInstaller
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_giant_free_server_1_1_i_giant_free_server.html")]
    [RequireComponent(typeof(NetworkManagerInstaller))]
    [DisallowMultipleComponent]
    public sealed class GiantFreeServerInstaller : ServiceInstaller<IGiantFreeServer>
    {
        protected override IService Create()
        {
            return new GiantFreeServer();
        }
    }
}