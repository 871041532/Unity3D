// -----------------------------------------------------------------
// File:    Dropdown.cs
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
    class Dropdown : Element, IDropDown
    {
        public Dropdown(string path, RectTransform transform)
            : base(path, transform, UIType.DROPDOWN)
        {
            this.dropdown = this.transform.GetComponent<UnityEngine.UI.Dropdown>();
            this.dropdown.onValueChanged.AddListener(_OnClickListener);
        }

        public int Value
        {
            get {
                return this.dropdown.value;
            }
        }

        private void _OnClickListener(int index)
        {
            for (var i = 0; i < this.handlers.Count; ++i) {
                this.handlers[i](index);
            }
        }

        public void OnClick(Action<int> handler)
        {
            var index = this.handlers.IndexOf(handler);
            if (index < 0) {
                this.handlers.Add(handler);
            }
        }

        public void AddOption(string option)
        {
            this.dropdown.options.Add(new UnityEngine.UI.Dropdown.OptionData(option));
        }

        public void ClearOptions()
        {
            this.dropdown.ClearOptions();
        }

        public string GetOption(int index)
        {
            if (this.dropdown.options.Count > index)
                return this.dropdown.options[index].text;

            return string.Empty;
        }

        internal override void _Reset()
        {
            this.dropdown.onValueChanged.RemoveListener(_OnClickListener);
            this.handlers.Clear();

            base._Reset();
        }

        private UnityEngine.UI.Dropdown dropdown = null;
        private List<Action<int>> handlers = new List<Action<int>>();
    }
}
