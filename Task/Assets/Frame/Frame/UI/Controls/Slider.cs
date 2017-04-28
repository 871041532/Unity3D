// -----------------------------------------------------------------
// File:    Slider.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.UI
{
    class Slider : Element, ISlider
    {
        public Slider(string path, RectTransform transform)
            : base(path, transform, UIType.SLIDER)
        {
            this.slider = this.transform.GetComponent<UnityEngine.UI.Slider>();
        }

        public float Value
        {
            get {
                return this.slider.value;
            }
            set {
                this.slider.value = value;
            }
        }

        private UnityEngine.UI.Slider slider = null;
    }
}
