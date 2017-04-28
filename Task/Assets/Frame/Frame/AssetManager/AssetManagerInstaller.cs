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
    /// @details �ʲ��������װ����
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�IAssetManager
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.assetmanager
    /// 
    /// @see IAssetManager
    /// 
    /// @section dependencies ����
    /// �ޡ�
    /// 
    /// @section howtouse ʹ��
    /// ֱ����ק��ServicePlayer���ڵ�GameObject�ϼ��ɡ�
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