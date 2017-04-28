// -----------------------------------------------------------------
// File:    BuglyInstaller.cs
// Author:  mouguangyi
// Date:    2017.02.04
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using UnityEngine;

namespace GameBox.Service.Bugly
{
    /// <summary>
    /// @details Bugly����İ�װ����
    /// </summary>
    [HelpURL("https://bugly.qq.com")]
    [RequireComponent(typeof(NativeChannelInstaller))]
    [DisallowMultipleComponent]
    public sealed class BuglyInstaller : ServiceInstaller<IBugly>
    {
        /// <summary>
        /// IOSƽ̨��App Id��
        /// </summary>
        public string AppIdForIOS;

        /// <summary>
        /// Androidƽ̨��App Id��
        /// </summary>
        public string AppIdForAndroid;

        /// <summary>
        /// 
        /// </summary>
        public bool IsDebug = false;

        protected override IService Create()
        {
            return new Bugly();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("AppIdForIOS", this.AppIdForIOS);
            args.Set("AppIdForAndroid", this.AppIdForAndroid);
            args.Set("IsDebug", this.IsDebug);
        }
    }
}