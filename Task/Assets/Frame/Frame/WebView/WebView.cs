// -----------------------------------------------------------------
// File:    WebView.cs
// Author:  ruanban
// Date:    2017.02.15
// Description:
//      
// -----------------------------------------------------------------

using System;
using UnityEngine;
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using System.Reflection;

namespace GameBox.Service.WebView
{
    sealed class WebView : IWebView
    {

        public string Id
        {
            get
            {
                return "com.giant.service.webview";
            }
        }

        public string name { set;get; }

        public void Run(IServiceRunner runner)
        {
            new ServiceTask<INativeChannel>().Start().Continue(task =>
            {
                var nativeChannel = task.Result as INativeChannel;
                this.webView = nativeChannel.Connect(new NativeModule
                {
                    IOS = "CWebViewPlugin",
                    Android = "com.giant.libwebview.CWebViewPlugin",
                }, this);
                this.runner = runner;
                this.runner.Ready(_Terminate);
                return null;
            });
        }

        public void Pulse(float dtTime)
        {
            if (IsBinded())
                Transform(bindRectT,bindCanvas);
        }

        public void Open(RectTransform t, object owner = null, bool bind = false)
        {
            this.Close();
            this.owner = owner;
            this.webView.Call("Init",  false, this.runner.GetArgs<bool>("IsUseWebKit"));
            this.Visible = true;
            this.isOpen = true;

            var defaultURL = this.runner.GetArgs<string>("DefaultURL");
            if (!string.IsNullOrEmpty(defaultURL))
                LoadURL(defaultURL);
            if (bind)
                this.Bind(t);
            else
                this.Transform(t);
        }

        public void Transform(RectTransform t, Canvas canvas = null)
        {
            if (t == null)
                throw new ArgumentNullException("t");

            canvas = canvas != null ? canvas : t.GetComponentInParent<Canvas>();
            if (canvas != null && (canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
                canvas.renderMode == RenderMode.ScreenSpaceCamera))
            {
                t.GetWorldCorners(transformBuffer);
                var camera = canvas.worldCamera;
                if (canvas.renderMode == RenderMode.ScreenSpaceCamera && camera != null)
                {
                    transformBuffer[0] = camera.WorldToScreenPoint(transformBuffer[0]);
                    transformBuffer[2] = camera.WorldToScreenPoint(transformBuffer[2]);
                }
                _setMargins((int)transformBuffer[0].x, Screen.height - (int)transformBuffer[2].y, 
                    Screen.width - (int)transformBuffer[2].x, (int)transformBuffer[0].y);
            }
        }

        public bool IsBinded()
        {
            return bindRectT != null;
        }

        public void Bind(RectTransform t)
        {
            if (t == null)
                throw new ArgumentNullException("t");
            bindCanvas = t.GetComponentInParent<Canvas>();
            if (bindCanvas == null)
                throw new NullReferenceException(string.Format("bind object[{0}] must be in canvas!", t.name));
            bindRectT = t;
        }

        public void UnBind()
        {
            Transform(bindRectT, bindCanvas);
            bindRectT = null;
            bindCanvas = null;
        }

        public void Close()
        {
            if (this.webView == null)
                return;
            this.webView.Call("Destroy");
            this.mIsKeyboardVisible = false;
            this.visibility = false;
            this.bindRectT = null;
            this.isOpen = false;
        }

        private void _Terminate()
        {
            Close();
            if (this.webView != null)
                this.webView.Disconnect();
        }

        public bool KeyboardVisible
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                    return this.mIsKeyboardVisible;
                else
                    return TouchScreenKeyboard.visible;
            }
        }

        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        public bool Visible
        {
            set
            {
                if (webView == null)
                    return;
                webView.Call("SetVisibility", value);
                visibility = value;
            }
            get
            {
                return visibility;
            }
        }

        public void LoadURL(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;

            if (webView == null)
                return;
            webView.Call("LoadURL", url);
        }

        public void LoadHTML(string html, string baseUrl)
        {
            if (string.IsNullOrEmpty(html))
                return;
            if (string.IsNullOrEmpty(baseUrl))
                baseUrl = "";
            if (webView == null)
                return;
            webView.Call("LoadHTML", html, baseUrl);
        }

        public void Reload()
        {
            if (webView == null)
                return;
            webView.Call("Reload");
        }

        public void EvaluateJS(string js)
        {
            if (webView == null)
                return;
            webView.Call("EvaluateJS", js);
        }

        public bool CanGoBack()
        {
            return this.canGoBack;
        }

        public bool CanGoForward()
        {
            return this.canGoForward;
        }

        public void GoBack()
        {
            if (webView == null)
                return;
            webView.Call("GoBack");
        }

        public void GoForward()
        {
            if (webView == null)
                return;
            webView.Call("GoForward");
        }

        private void SetCanGoBack(bool bCan)
        {
            this.canGoBack = bCan;
        }

        private void SetCanGoForward(bool bCan)
        {
            this.canGoForward = bCan;
        }

        private void OnWebViewError(string error)
        {
            _sendMessageToOwner("OnWebViewError", error);
        }

        private void OnWebViewLoad(string url)
        {
            _sendMessageToOwner("OnWebViewLoad", url);
        }

        private void OnWebViewJS(string json)
        {
            if (this.owner != null && !string.IsNullOrEmpty(json))
            {
                //var proto = SimpleJson.SimpleJson.DeserializeObject<CallProtocal>(json);
               // _sendMessageToOwner(proto);
            }
        }

        private void _sendMessageToOwner(CallProtocal call)
        {
            var classType = this.owner.GetType();
            var methodInfo = classType.GetMethod(call.Method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (null != methodInfo)
            {
                //if (call.Args != null)
                //{
                //    for (int i = 0; i < call.Args.Length; ++i)
                //    {
                //        if (call.Args[i].GetType() == typeof(Int64))
                //        {
                //            call.Args[i
                //        }
                //    }
                //}
                var ps = methodInfo.GetParameters();
                if (call.Args != null && call.Args.Length > 0 && ps.Length == call.Args.Length)
                {
                    for (int i = 0; i < ps.Length; ++i)
                    {
                        call.Args[i] = System.Convert.ChangeType(call.Args[i], ps[i].ParameterType);
                    }
                }
                methodInfo.Invoke(this.owner, call.Args);
            }
        }

        private void _sendMessageToOwner(string msg,string param)
        {
            if (this.owner != null)
            {
                var protoc = new CallProtocal();
                protoc.Method = msg;
                protoc.Args = new object[1] { param };
                _sendMessageToOwner(protoc);
            }
        }

        private void _setMargins(int left, int top, int right, int bottom)
        {
            if (webView == null)
                return;
            webView.Call("SetMargins", left, top, right, bottom);
        }

        private object owner;
        private RectTransform bindRectT;
        private Canvas bindCanvas;
        private Vector3[] transformBuffer = new Vector3[4];
        private IServiceRunner runner;
        private bool visibility;
        private bool canGoBack = false;
        private bool canGoForward = false;
        private bool isOpen = false;

        private class CallProtocal
        {
            public string Method = "";
            public object[] Args = null;
        }

        private INativeProxy webView;
        private bool mIsKeyboardVisible = false;
        /// Called from Java native plugin to set when the keyboard is opened
        private void SetKeyboardVisible(string pIsVisible)
        {
            mIsKeyboardVisible = (pIsVisible == "true");
        }
    }
}
