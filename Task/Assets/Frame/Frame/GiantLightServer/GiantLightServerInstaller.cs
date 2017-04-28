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
    /// @details �����������������Խӷ���װ����
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�IGiantLightServer
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.giantlightserver
    /// 
    /// @see IGiantLightServer
    ///  
    /// @section dependences ����
    /// @li @c NetworkManagerInstaller
    /// 
    /// @section howtouse ʹ��
    /// ֱ����ק��ServicePlayer���ڵ�GameObject�ϼ��ɡ�
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