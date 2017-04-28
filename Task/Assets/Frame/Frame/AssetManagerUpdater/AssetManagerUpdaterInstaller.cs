// -----------------------------------------------------------------
// File:    AssetManagerUpdaterInstaller.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using GameBox.Service.AssetUpdater;
using UnityEngine;

namespace GameBox.Service.AssetManagerUpdater
{
    /// <summary>
    /// @details �ʲ������ȸ��·���
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�IAssetManagerUpdater
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.assetmanagerupdater
    /// 
    /// @see IAssetManagerUpdater
    /// 
    /// @section dependences ����
    /// @li @c AssetManagerInstaller
    /// @li @c AssetListUpdaterInstaller
    /// 
    /// @section howtouse ʹ��
    /// ֱ����ק��ServicePlayer���ڵ�GameObject�ϼ��ɡ�
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_asset_manager_updater_1_1_i_asset_manager_updater.html")]
    [RequireComponent(typeof(AssetManagerInstaller))]
    [RequireComponent(typeof(AssetListUpdaterInstaller))]
    [DisallowMultipleComponent]
    public sealed class AssetManagerUpdaterInstaller : ServiceInstaller<IAssetManagerUpdater>
    {
        /// <summary>
        /// Ӧ��ǿ���µİ�װ�����ط�������ַ��
        /// </summary>
        public string AppServer;

        /// <summary>
        /// Ӧ�õ��ȸ�����Դ��������ַ��
        /// </summary>
        public string AssetServer;

        /// <summary>
        /// �����Ƿ���Ч��
        /// 
        /// @note �ڿ����׶�����Ϊfalse���Ա��⿪���д����ȸ������̡�
        /// </summary>
        public bool Valid = true;

        protected override IService Create()
        {
            return new AssetManagerUpdater();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("AppServer", this.AppServer);
            args.Set("AssetServer", this.AssetServer);
            args.Set("Valid", this.Valid);
        }
    }
}