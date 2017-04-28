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
    /// @details ���˶��η������������װ������
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�IGiantFreeServer
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.giantfreeserver
    /// 
    /// @see IGiantFreeServer
    ///  
    /// @section dependences ����
    /// @li @c NetworkManagerInstaller
    /// 
    /// @section howtouse ʹ��
    /// ֱ����ק��ServicePlayer���ڵ�GameObject�ϼ��ɡ�
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