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
    /// @details �ʲ��ļ��б��ȸ��·����������
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�IAssetListUpdater
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.assetlistupdater
    /// 
    /// @see IAssetListUpdater
    ///  
    /// @section dependences ����
    /// �ޡ�
    /// 
    /// @section howtouse ʹ��
    /// ֱ����ק��ServicePlayer���ڵ�GameObject�ϼ��ɡ�
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