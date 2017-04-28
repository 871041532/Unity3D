// -----------------------------------------------------------------
// File:    Toggle.cs
// Author:  mouguangyi
// Date:    2017.01.17
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameBox.Service.UI
{
    class Toggle : Element, IToggle
    {
        public Toggle(string path, RectTransform transform)
            : base(path, transform)
        {
            this.toggle = this.transform.GetComponent<UnityEngine.UI.Toggle>();
            this.normalSprite = ((UnityEngine.UI.Image)this.toggle.targetGraphic).sprite;
            this.toggle.onValueChanged.AddListener(value =>
            {
                _UpdateToggleState();
                if (null != this.handler) {
                    this.handler(this.toggle.isOn);
                }
            });
        }

        public void OnChange(Action<bool> handler)
        {
            this.handler = handler;
        }

        public bool On
        {
            get {
                return this.toggle.isOn;
            }
            set {
                this.toggle.isOn = value;
                _UpdateToggleState();
            }
        }

        internal override void _Reset()
        {
            this.toggle.onValueChanged.RemoveAllListeners();
            this.toggle.isOn = false;
            this.toggle.image.sprite = this.normalSprite;

            base._Reset();
        }

        private void _UpdateToggleState()
        {
            switch (this.toggle.transition) {
            case Selectable.Transition.SpriteSwap:
                this.toggle.image.sprite = this.toggle.isOn ? this.toggle.spriteState.highlightedSprite : this.normalSprite;
                break;
            }
        }

        private UnityEngine.UI.Toggle toggle = null;
        private Sprite normalSprite = null;
        private Action<bool> handler = null;
    }
}