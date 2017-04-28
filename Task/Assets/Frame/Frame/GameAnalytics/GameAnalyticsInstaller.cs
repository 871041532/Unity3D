// -----------------------------------------------------------------
// File:    GameAnalyticsInstaller.cs
// Author:  liuwei
// Date:    2017.03.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using UnityEngine;

namespace GameBox.Service.GameAnalytics
{
    /// <summary>
    /// @details 数据分析服务的安装器。
    /// </summary>
    [RequireComponent(typeof(NativeChannelInstaller))]
    [DisallowMultipleComponent]
    public sealed class GameAnalyticsInstaller : ServiceInstaller<IGameAnalytics>
    {
        /// <summary>
        /// Application Id。
        /// </summary>
        public string AppId = "88EAAE90907544798E1A3AB197B747A5";

        /// <summary>
        /// Application Channel。
        /// </summary>
        public string AppChannel = "com.giant.gameanalytics";

        protected override IService Create()
        {
            return new GameAnalytics();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("AppId", this.AppId);
            args.Set("AppChannel", this.AppChannel);
        }
    }
}