// -----------------------------------------------------------------
// File:    Label.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace GameBox.Service.UI
{
    class Label : Element, ILabel
    {
        public Label(string path, RectTransform transform)
            : base(path, transform, UIType.LABEL)
        {
            this.text = this.transform.GetComponent<Text>();
            if (this.text is Extension.HyperText)
            {
                (this.text as Extension.HyperText).OnHrefClick.AddListener(_OnClickListener);
            }  
        }

        public string Text
        {
            get {
                return this.text.text;
            }
            set {
                this.text.text = value;
            }
        }

        public Color Color
        {
            get {
                return this.text.color;
            }
            set {
                this.text.color = value;
            }
        }

        public int FontSize
        {
            get {
                return this.text.fontSize;
            }
            set {
                this.text.fontSize = value;
            }
        }

        public void OnClick(Action<string> handler)
        {
            var index = this.handlers.IndexOf(handler);
            if (index < 0)
            {
                this.handlers.Add(handler);
            }
        }

        internal override void _Reset()
        {
            if (this.text is Extension.HyperText)
            {
                (this.text as Extension.HyperText).OnHrefClick.RemoveListener(_OnClickListener);
                this.handlers.Clear();
            }
            base._Reset();
        }

        public HrefClickEvent OnHrefClick
        {
            get
            {
                Extension.HyperText tp = (this.text as Extension.HyperText);
                if(tp != null)
                    return tp.OnHrefClick;
                return null;
            }
            set
            {
                Extension.HyperText tp = (this.text as Extension.HyperText);
                if (tp != null)
                    tp.OnHrefClick = value;
            }
        }

        private void _OnClickListener(string hrefInfo)
        {
            for (var i = 0; i < this.handlers.Count; ++i)
            {
                this.handlers[i](hrefInfo);
            }
        }

        private List<Action<string>> handlers = new List<Action<string>>();

        private Text text = null;
    }
}
