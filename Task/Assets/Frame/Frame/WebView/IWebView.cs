// -----------------------------------------------------------------
// File:    IWebView.cs
// Author:  ruanban
// Date:    2017.02.15
// Description:
//      
// -----------------------------------------------------------------

using UnityEngine;
using GameBox.Framework;

namespace GameBox.Service.WebView
{
    public interface IWebView : IService
    {
        /// <summary>
        /// 打开一个webview 界面
        /// OnWebViewLoad(string url) 和 OnWebViewError(string error)是预定义的两个事件
        /// 当加载页面完成时会调到owner的OnWebViewLoad函数
        /// 当webvew发生错误的时候会调到owner的 OnWebViewError
        /// 如果希望在js中调用某个功能只需要在 owner中实现相应的函数(如:void test(int a)),然后在
        /// html中的js中调用Unity.Call("test",100),就会调到 ownner的改法.最多支持一个[int|int64|float|double|string]类型的参数.
        /// </summary>
        /// <param name="t">和t同步在屏幕中的大小,位置</param>
        /// <param name="owner">webview事件和js事件的接收者</param>
        /// <param name="bind">如果为true,会每一帧和t执行同步操作(为了实现动画)</param>
        void Open(RectTransform t, object owner = null, bool bind = false);
        /// <summary>
        /// webview 是否呼出了键盘
        /// </summary>
        bool KeyboardVisible { get; }
        /// <summary>
        /// webview是否打开
        /// </summary>
        bool IsOpen { get; }
        /// <summary>
        /// 控制WebView显示|隐藏
        /// </summary>
        bool Visible { set; get; }
        /// <summary>
        /// 加载一个url地址
        /// </summary>
        /// <param name="url">url地址,例如:http://www.ztgame.com</param>
        void LoadURL(string url);
        /// <summary>
        /// 加载一个html网页
        /// </summary>
        /// <param name="html">html网页内容</param>
        /// <param name="baseUrl">html内容中的链接,如:js,css,所在的地址。如果在本地填空</param>
        void LoadHTML(string html, string baseUrl);
        /// <summary>
        /// 重新加载当前页面
        /// </summary>
        void Reload();
        /// <summary>
        /// 让WebView执行一段Js代码
        /// </summary>
        /// <param name="js">js代码</param>
        void EvaluateJS(string js);
        /// <summary>
        /// 判断是否能后退
        /// </summary>
        /// <returns>true表示能,false表示不能</returns>
        bool CanGoBack();
        /// <summary>
        /// 判断是否能前进
        /// </summary>
        /// <returns>true表示能,false表示不能</returns>
        bool CanGoForward();
        /// <summary>
        /// 让webview执行后退操作
        /// </summary>
        void GoBack();
        /// <summary>
        /// 让webview执行前进操作
        /// </summary>
        void GoForward();
        /// <summary>
        /// 进行一次变换同步
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="t"></param>
        void Transform(RectTransform t, Canvas canvas = null);
        /// <summary>
        /// 是否绑定了某个RectTransform对象
        /// </summary>
        /// <returns>true表示绑定了,false表示没绑定</returns>
        bool IsBinded();
        /// <summary>
        /// 绑定到 某个RectTransform对象上去,绑定后会每一帧与该RectTransform对象在屏幕上所在区域保持一致
        /// </summary>
        /// <param name="t"></param>
        void Bind(RectTransform t);
        /// <summary>
        /// 解除绑定
        /// </summary>
        void UnBind();
        /// <summary>
        /// 关闭浏览器,在不用的时候需要调用Close来销毁
        /// </summary>
        void Close();
    }
}
