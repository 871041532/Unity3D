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
    /// @details ����������İ�װ����
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�INetworkManager
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.networkmanager
    /// 
    /// @see INetworkManager
    /// 
    /// @section dependencies ����
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