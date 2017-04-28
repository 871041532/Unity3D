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
        /// ��һ��webview ����
        /// OnWebViewLoad(string url) �� OnWebViewError(string error)��Ԥ����������¼�
        /// ������ҳ�����ʱ�����owner��OnWebViewLoad����
        /// ��webvew���������ʱ������owner�� OnWebViewError
        /// ���ϣ����js�е���ĳ������ֻ��Ҫ�� owner��ʵ����Ӧ�ĺ���(��:void test(int a)),Ȼ����
        /// html�е�js�е���Unity.Call("test",100),�ͻ���� ownner�ĸķ�.���֧��һ��[int|int64|float|double|string]���͵Ĳ���.
        /// </summary>
        /// <param name="t">��tͬ������Ļ�еĴ�С,λ��</param>
        /// <param name="owner">webview�¼���js�¼��Ľ�����</param>
        /// <param name="bind">���Ϊtrue,��ÿһ֡��tִ��ͬ������(Ϊ��ʵ�ֶ���)</param>
        void Open(RectTransform t, object owner = null, bool bind = false);
        /// <summary>
        /// webview �Ƿ�����˼���
        /// </summary>
        bool KeyboardVisible { get; }
        /// <summary>
        /// webview�Ƿ��
        /// </summary>
        bool IsOpen { get; }
        /// <summary>
        /// ����WebView��ʾ|����
        /// </summary>
        bool Visible { set; get; }
        /// <summary>
        /// ����һ��url��ַ
        /// </summary>
        /// <param name="url">url��ַ,����:http://www.ztgame.com</param>
        void LoadURL(string url);
        /// <summary>
        /// ����һ��html��ҳ
        /// </summary>
        /// <param name="html">html��ҳ����</param>
        /// <param name="baseUrl">html�����е�����,��:js,css,���ڵĵ�ַ������ڱ������</param>
        void LoadHTML(string html, string baseUrl);
        /// <summary>
        /// ���¼��ص�ǰҳ��
        /// </summary>
        void Reload();
        /// <summary>
        /// ��WebViewִ��һ��Js����
        /// </summary>
        /// <param name="js">js����</param>
        void EvaluateJS(string js);
        /// <summary>
        /// �ж��Ƿ��ܺ���
        /// </summary>
        /// <returns>true��ʾ��,false��ʾ����</returns>
        bool CanGoBack();
        /// <summary>
        /// �ж��Ƿ���ǰ��
        /// </summary>
        /// <returns>true��ʾ��,false��ʾ����</returns>
        bool CanGoForward();
        /// <summary>
        /// ��webviewִ�к��˲���
        /// </summary>
        void GoBack();
        /// <summary>
        /// ��webviewִ��ǰ������
        /// </summary>
        void GoForward();
        /// <summary>
        /// ����һ�α任ͬ��
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="t"></param>
        void Transform(RectTransform t, Canvas canvas = null);
        /// <summary>
        /// �Ƿ����ĳ��RectTransform����
        /// </summary>
        /// <returns>true��ʾ����,false��ʾû��</returns>
        bool IsBinded();
        /// <summary>
        /// �󶨵� ĳ��RectTransform������ȥ,�󶨺��ÿһ֡���RectTransform��������Ļ���������򱣳�һ��
        /// </summary>
        /// <param name="t"></param>
        void Bind(RectTransform t);
        /// <summary>
        /// �����
        /// </summary>
        void UnBind();
        /// <summary>
        /// �ر������,�ڲ��õ�ʱ����Ҫ����Close������
        /// </summary>
        void Close();
    }
}
