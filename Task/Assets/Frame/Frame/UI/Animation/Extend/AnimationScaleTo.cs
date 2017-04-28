// -----------------------------------------------------------------
// File:    AnimationScaleTo.cs
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
    class AnimationScaleTo: AnimationInterval
    {
        protected Vector3 startScale;
        protected Vector3 endScale;

        public AnimationScaleTo(float duration, float endScale) :base(duration)
        {
            this.endScale = new Vector3(endScale, endScale, endScale);
        }

        public override void Start()
        {
            base.Start();
            this.startScale = this.Transform.localScale;
        }

        public override void Update(float ratio)
        {
            this.Transform.localScale = Vector3.Lerp(this.startScale, this.endScale, ratio);
        }
    }
}
