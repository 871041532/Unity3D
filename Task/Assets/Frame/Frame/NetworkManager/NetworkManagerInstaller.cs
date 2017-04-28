// -----------------------------------------------------------------
// File:    NetworkManagerInstaller.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.ByteStorage;
using UnityEngine;

namespace GameBox.Service.NetworkManager
{
    /// <summary>
    /// @details 网络管理服务的安装器。
    /// 
    /// @li @c 对应的服务接口：INetworkManager
    /// @li @c 对应的服务ID：com.giant.service.networkmanager
    /// 
    /// @see INetworkManager
    /// 
    /// @section dependencies 依赖
    /// @li @c ByteStorageInstaller
    ///  
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_network_manager_1_1_i_network_manager.html")]
    [RequireComponent(typeof(ByteStorageInstaller))]
    [DisallowMultipleComponent]
    public sealed class NetworkManagerInstaller : ServiceInstaller<INetworkManager>
    {
        protected override IService Create()
        {
            return new NetworkManager();
        }
    }
}