// -----------------------------------------------------------------
// File:    GiantLightServerInstaller.cs
// Author:  mouguangyi
// Date:    2016.06.16
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NetworkManager;
using UnityEngine;

namespace GameBox.Service.GiantLightServer
{
    /// <summary>
    /// @details 巨人轻量级服务器对接服务安装器。
    /// 
    /// @li @c 对应的服务接口：IGiantLightServer
    /// @li @c 对应的服务ID：com.giant.service.giantlightserver
    /// 
    /// @see IGiantLightServer
    ///  
    /// @section dependences 依赖
    /// @li @c NetworkManagerInstaller
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_giant_light_server_1_1_i_giant_light_server.html")]
    [RequireComponent(typeof(NetworkManagerInstaller))]
    [DisallowMultipleComponent]
    public sealed class GiantLightServerInstaller : ServiceInstaller<IGiantLightServer>
    {
        protected override IService Create()
        {
            //return new GiantLightServer();
            return null;
        }
    }
}