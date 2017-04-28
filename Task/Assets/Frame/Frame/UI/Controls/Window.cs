// -----------------------------------------------------------------
// File:    Window.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.AssetManager;
using System.Collections.Generic;
using UnityEngine;

namespace GameBox.Service.UI
{
    class Window : Element, IWindow
    {
        public Window(UISystem system, IAsset asset, string path, RectTransform transform)
            : base(path, transform, UIType.WINDOW)
        {
            this.system = system;
            this.asset = asset;
            this.original = this.transform.localPosition;
            this.window = this;
            this.system._AddWindow(this);
        }

        public override void Dispose()
        {
            this.asset.Dispose();

            if (null != this.transform) {
                GameObject.Destroy(this.transform.gameObject);
            }

            this.system = null;
            this.asset = null;

            base.Dispose();
        }

        public void Close()
        {
            this.system._RemoveWindow(this);
        }

        public void Recover()
        {
            //this.transform.localPosition = this.original;
            this.transform.gameObject.SetActive(true);
        }

        public void Reclaim()
        {
            foreach (var element in this.elements.Values) {
                element._Reset();
            }
            this.elements.Clear();

            //this.transform.localPosition = INVISIBLE_POSITION;
            this.transform.gameObject.SetActive(false);
        }

        public override void Update(float delta)
        {
            if (this.animationElements.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < this.animationElements.Count; i++)
            {
                this.animationElements[i].Update(delta);
            }
        }

        public void AddAnimationElement(Element element)
        {
            var index = this.animationElements.IndexOf(element);
            if (index < 0)
            {
                this.animationElements.Add(element);
            }
            this.system.AddAnimationWindow(this);
        }

        public void RemoveAnimationElement(Element element)
        {
            this.animationElements.Remove(element);
            if(this.animationElements.Count == 0)
            {
                this.system.RemoveAnimationWindow(this);
            }
        }

        internal Element _FindElement(string path, int type)
        {
            Element element = null;
            if (this.elements.TryGetValue(path, out element)) {
                return element;
            } else {
                var childTransform = this.transform.Find(path) as RectTransform;
                if (null != childTransform) {
                    element = this.system._CreateElement(path, type, childTransform);
                    element.window = this;
                    this.elements.Add(path, element);
                    return element;
                }
            }

            return null;
        }

        internal Transform _Transform
        {
            get {
                return this.transform;
            }
        }

        private UISystem system = null;
        private Vector3 original;
        private IAsset asset = null;
        private Dictionary<string, Element> elements = new Dictionary<string, Element>();
        private List<Element> animationElements = new List<Element>();
        private static Vector3 INVISIBLE_POSITION = new Vector3(-10000f, -10000f, 0f);
    }
}

