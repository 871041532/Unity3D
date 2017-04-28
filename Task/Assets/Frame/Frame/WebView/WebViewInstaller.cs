// -----------------------------------------------------------------
// File:    WebViewInstaller.cs
// Author:  ruanban
// Date:    2017.02.15
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
#if !UNITY_ANDROID
using UnityEngine;
#endif

namespace GameBox.Service.WebView
{
	public sealed class WebViewInstaller : ServiceInstaller<IWebView>
	{
        /// <summary>
        /// 是否使用webkit内核,仅IPhone平台
        /// </summary>
        public bool IsUseWebKit = true;

        public string DefaultURL = "";

        protected override IService Create()
		{
            return new WebView();
		}

        protected override void Arguments(IServiceArgs args)
        {
            args.Set<bool>("IsUseWebKit", IsUseWebKit);
            args.Set<string>("DefaultURL", this.DefaultURL);
            base.Arguments(args);
        }
    }
}