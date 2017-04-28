// -----------------------------------------------------------------
// File:    Toggles.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameBox.Service.UI
{
    class Toggles : Element, IToggles
    {
        public Toggles(string path, RectTransform transform)
            : base(path, transform, UIType.TOGGLES)
        {
            this.group = this.transform.GetComponent<ToggleGroup>();
            var childToggles = this.transform.GetComponentsInChildren<UnityEngine.UI.Toggle>();
            for (var i = 0; i < childToggles.Length; ++i) {
                var toggle = childToggles[i];
                if (toggle.group == this.group) {
                    var togglePath = this.Path + "/" + i;
                    var element = new Toggle(togglePath, toggle.transform as RectTransform);
                    element.OnChange(_OnToggleOn);

                    this.toggles.Add(element);  // DO NOT record in window
                }
            }

            if (this.group.allowSwitchOff) {
                this.group.SetAllTogglesOff();
            }
        }

        public void AddToggleChange(Action handler)
        {
            this.handlers.Add(handler);
        }

        public int ToggleOn
        {
            get {
                for (var i = 0; i < this.toggles.Count; ++i) {
                    if (this.toggles[i].On) {
                        return i;
                    }
                }

                return -1;
            }
            set {
                if (value >= 0 && value < this.toggles.Count) {
                    this.toggles[value].On = true;
                }
            }
        }

        internal override void _Reset()
        {
            for (var i = 0; i < this.toggles.Count; ++i) {
                this.toggles[i]._Reset();
            }
            this.toggles.Clear();

            base._Reset();
        }

        private void _OnToggleOn(bool isOn)
        {
            if (isOn) {
                for (var i = 0; i < this.handlers.Count; ++i) {
                    this.handlers[i]();
                }
            }
        }

        private ToggleGroup group = null;
        private List<Toggle> toggles = new List<Toggle>();
        private List<Action> handlers = new List<Action>();
    }
}

