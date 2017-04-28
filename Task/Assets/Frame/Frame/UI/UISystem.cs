// -----------------------------------------------------------------
// File:    UISystem.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using GameBox.Service.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameBox.Service.UI
{
    class UISystem : IUISystem, IRecycleProcesser
    {
        #region IService
        public string Id
        {
            get {
                return "com.giant.service.uisystem";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServicesTask(new string[] {
                "com.giant.service.assetmanager",
                "com.giant.service.objectpool"
            }).Start().Continue(task =>
            {
                var services = task.Result as IService[];
                this.assetManager = services[0] as IAssetManager;
                this.pool = (services[1] as IRecycleManager).Create("GameBox.Service.UI.Window", this);

                var rootObj = new GameObject("_UIManager");

                //var cameraObj = new GameObject("_UICamera", new[] { typeof(Camera) });
                //var camera = cameraObj.GetComponent<Camera>();
                //camera.orthographic = true;
                //camera.farClipPlane = 50f;
                //camera.cullingMask = 1 << 8;

                var canvas = rootObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                //canvas.worldCamera = camera;

                var scaler = rootObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(640f, 1136f);
                scaler.matchWidthOrHeight = 1f;
                rootObj.AddComponent<GraphicRaycaster>();

                this.root = rootObj.transform;

                //var transform = cameraObj.transform;
                //transform.SetParent(this.root);
                //transform.localPosition = new Vector3(0f, 0f, -100f);

                runner.Ready(_Terminate);
                return null;
            });
        }

        public void Pulse(float delta)
        {
            for(int i=0; i< this.animationWindows.Count;i++)
            {
                this.animationWindows[i].Update(delta);
            }
        }


        public void AddAnimationWindow(Window window)
        {
            var index = this.animationWindows.IndexOf(window);
            if (index < 0)
            {
                this.animationWindows.Add(window);
            }
        }

        public void RemoveAnimationWindow(Window window)
        {
            this.animationWindows.Remove(window);
        }
        #endregion

        #region IUISystem
        public IWindow CreateWindow(string path, int layerIndex)
        {
            var window = this.pool.Pick<Window>(path);
            if (null != window) {
                _AddWindow(window);
            } else {
                var asset = this.assetManager.Load(path, AssetType.PREFAB);
                var go = GameObject.Instantiate(asset.Cast<GameObject>()) as GameObject;
                window = new Window(this, asset, path, go.transform as RectTransform);
            }

            _InitWindowWithLayer(window, layerIndex);

            return window;
        }

        public void CreateWindowAsync(string path, int layerIndex, Action<IWindow> callback)
        {
            var window = this.pool.Pick<Window>(path);
            if (null != window) {
                new CompletedTask().Start().Continue(task =>
                {
                    _AddWindow(window);
                    _InitWindowWithLayer(window, layerIndex);
                    _NotifyWindowCallback(window, callback);
                    return null;
                });
            } else {
                this.assetManager.LoadAsync(path, AssetType.PREFAB, asset =>
                {
                    var go = GameObject.Instantiate(asset.Cast<GameObject>()) as GameObject;
                    window = new Window(this, asset, path, go.transform as RectTransform);
                    _InitWindowWithLayer(window, layerIndex);
                    _NotifyWindowCallback(window, callback);
                });
            }
        }

        public IWindow FindWindow(string path)
        {
            List<Window> pathWindows = null;
            if (this.windows.TryGetValue(path, out pathWindows)) {
                return pathWindows.Count > 0 ? pathWindows[0] : null;
            } else {
                return null;
            }
        }

        public IWindow[] FindWindows(string path)
        {
            List<Window> pathWindows = null;
            if (this.windows.TryGetValue(path, out pathWindows)) {
                return pathWindows.ToArray();
            } else {
                return null;
            }
        }

        public void RegisterFactory(IControlFactory factory)
        {
            if (null != factory && this.factories.IndexOf(factory) < 0) {
                this.factories.Add(factory);
            }
        }
        #endregion

        #region IRecycleProcesser
        public void RecoverObject(object recycleObject)
        {
            //this.transform.localPosition = this.original;
            var window = (Window)recycleObject;
            window.Recover();
        }

        public void ReclaimObject(object recycleObject)
        {
            var window = (Window)recycleObject;
            window.Reclaim();
        }
        #endregion

        #region internal
        internal void _AddWindow(Window window)
        {
            List<Window> pathWindows = null;
            if (!this.windows.TryGetValue(window.Path, out pathWindows)) {
                this.windows[window.Path] = pathWindows = new List<Window>();
            }

            pathWindows.Add(window);
        }

        internal void _RemoveWindow(Window window)
        {
            List<Window> pathWindows = null;
            if (this.windows.TryGetValue(window.Path, out pathWindows)) {
                for (var i = 0; i < pathWindows.Count; ++i) {
                    if (pathWindows[i] == window) {
                        pathWindows.RemoveAt(i);
                        this.pool.Drop(window.Path, window);
                        break;
                    }
                }
            }
        }

        internal Element _CreateElement(string path, int type, RectTransform transform)
        {
            switch (type) {
            case UIType.LABEL:
                return new Label(path, transform);
            case UIType.IMAGE:
                return new Image(path, transform);
            case UIType.BUTTON:
                return new Button(path, transform);
            case UIType.INPUT:
                return new InputField(path, transform);
            case UIType.SCROLLVIEW:
                return new ScrollView(path, transform);
            case UIType.TOGGLE:
                return new Toggle(path, transform);
            case UIType.TOGGLES:
                return new Toggles(path, transform);
            case UIType.DROPDOWN:
                return new Dropdown(path, transform);
            case UIType.SLIDER:
                return new Slider(path, transform);
            case UIType.IMAGEANIMATION:
                return new ImageAnimation(path, transform);
            default:
                for (var i = 0; i < this.factories.Count; ++i) {
                    var element = this.factories[i].Create(type, path, transform) as Element;
                    if (null != element) {
                        return element;
                    }
                }

                return new Element(path, transform, UIType.ELEMENT);
            }
        }
        #endregion

        #region private
        private void _Terminate()
        { }

        private void _InitWindowWithLayer(Window window, int layerIndex)
        {
            var transform = window._Transform as RectTransform;
            transform.SetParent(_GetLayer(layerIndex));
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.sizeDelta = Vector2.zero;
            transform.SetAsLastSibling();
        }

        private Transform _GetLayer(int layerIndex)
        {
            if (layerIndex < 0) {
                return null;
            }

            if (layerIndex >= this.layers.Count) {
                var count = this.layers.Count;
                var needCount = layerIndex - this.layers.Count + 1;
                for (var i = 0; i < needCount; ++i) {
                    var layer = new GameObject("_layer" + (i + count), new Type[] { typeof(RectTransform) }).transform as RectTransform;
                    layer.SetParent(this.root);
                    layer.anchorMin = new Vector2(0, 0);
                    layer.anchorMax = new Vector2(1, 1);
                    layer.pivot = new Vector2(0.5f, 0.5f);
                    layer.sizeDelta = Vector2.zero;
                    layer.localPosition = Vector3.zero;
                    layer.localScale = Vector3.one;

                    this.layers.Add(layer);
                }
            }

            return this.layers[layerIndex];
        }

        private void _NotifyWindowCallback(IWindow window, Action<IWindow> callback)
        {
            if (null != callback) {
                callback(window);
            }
        }
        #endregion

        private IAssetManager assetManager = null;
        private IRecyclePool pool = null;
        private List<IControlFactory> factories = new List<IControlFactory>();
        private Transform root = null;
        private List<Transform> layers = new List<Transform>();
        private Dictionary<string, List<Window>> windows = new Dictionary<string, List<Window>>();
        private List<Window> animationWindows = new List<Window>();
    }
}
