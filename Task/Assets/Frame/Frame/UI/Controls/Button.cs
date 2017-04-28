// -----------------------------------------------------------------
// File:    Button.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameBox.Service.UI
{
    class Button : Element, IButton
    {
        public Button(string path, RectTransform transform)
            : base(path, transform, UIType.BUTTON)
        {
            this.button = this.transform.GetComponent<UnityEngine.UI.Button>();
            this.button.onClick.AddListener(_OnClickListener);
        }

        public override void OnClick(Action handler)
        {
            var index = this.handlers.IndexOf(handler);
            if (index < 0) {
                this.handlers.Add(handler);
            }
        }

        public bool Interactable
        {
            get {
                return this.button.interactable;
            }
            set {
                this.button.interactable = value;
            }
        }

        internal override void _Reset()
        {
            this.button.onClick.RemoveListener(_OnClickListener);
            this.handlers.Clear();

            base._Reset();
        }

        private void _OnClickListener()
        {
            for (var i = 0; i < this.handlers.Count; ++i) {
                this.handlers[i]();
            }
        }

        private UnityEngine.UI.Button button = null;
        private List<Action> handlers = new List<Action>();
    }
}