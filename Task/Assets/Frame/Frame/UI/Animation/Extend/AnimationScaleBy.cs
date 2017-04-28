// -----------------------------------------------------------------
// File:    AnimationScaleBy.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    class AnimationScaleBy: AnimationScaleTo
    {
        private Vector3 offsetScale;

        public AnimationScaleBy(float duration, float offsetScale) :base(duration, 0)
        {
            this.offsetScale = new Vector3(offsetScale, offsetScale, offsetScale);
        }

        public override void Start()
        {
            base.Start();
            this.endScale = new Vector3(this.startScale.x * this.offsetScale.x, this.startScale.y * this.offsetScale.y, this.startScale.z * this.offsetScale.z);
        }
    }
}
