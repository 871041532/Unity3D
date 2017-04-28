// -----------------------------------------------------------------
// File:    GiantVoiceInstaller.cs
// Author:  liuwei
// Date:    2017.02.07
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using GameBox.Service.UI;
using UnityEngine;

namespace GameBox.Service.GiantVoice
{
    /// <summary>
    /// @details 语音服务安装器。
    /// 
    /// @li @c 对应的服务接口：IGiantVoice
    /// @li @c 对应的服务ID：com.giant.service.giantvoice
    /// 
    /// @see IGiantVoice
    /// 
    /// </summary>
    //[HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_object_pool_1_1_i_object_pool.html")]
    [RequireComponent(typeof(UISystemInstaller))]
    [RequireComponent(typeof(NativeChannelInstaller))]
    public sealed class GiantVoiceInstaller : ServiceInstaller<IGiantVoice>
    {
        public int GameId = 100;//游戏应用id
        public long UserId = 100;//用户id
        public float MaxTime = 10.0f;
        public float MinTime = 0.5f;
        public string GameName = "giantvoice";//游戏名
        public string ZoneId = "test"; //服务器区id
        public string AndroidAppId = "56987548";
        public string IosAppId = "56987590";
        public string HttpServer = "http://qcloudsrv.ztgame.com.cn/voiceWeb/getconf.php";

        protected override IService Create()
        {
            return new GiantVoice();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("GameId", this.GameId);
            args.Set("UserId", this.UserId);
            args.Set("MaxTime", this.MaxTime);
            args.Set("MinTime", this.MinTime);
            args.Set("GameName", this.GameName);
            args.Set("ZoneId", this.ZoneId);
            args.Set("AndroidAppId", this.AndroidAppId);
            args.Set("IosAppId", this.IosAppId);
            args.Set("HttpServer", this.HttpServer);
        }
    }
}