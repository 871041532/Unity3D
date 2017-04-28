// -----------------------------------------------------------------
// File:    GetuiInstaller.cs
// Author:  mouguangyi
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using UnityEngine;

namespace GameBox.Service.Getui
{
    /// <summary>
    /// @details 个推服务的安装器。
    /// </summary>
    [HelpURL("http://www.getui.com/")]
    [RequireComponent(typeof(NativeChannelInstaller))]
    [DisallowMultipleComponent]
    public sealed class GetuiInstaller : ServiceInstaller<IGetui>
    {
        /// <summary>
        /// Application Id。
        /// </summary>
        public string AppId;

        /// <summary>
        /// Application Key。
        /// </summary>
        public string AppKey;

        /// <summary>
        /// Application Secret。
        /// </summary>
        public string AppSecret;

        protected override IService Create()
        {
            return new Getui();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("AppId", this.AppId);
            args.Set("AppKey", this.AppKey);
            args.Set("AppSecret", this.AppSecret);
        }
    }
}