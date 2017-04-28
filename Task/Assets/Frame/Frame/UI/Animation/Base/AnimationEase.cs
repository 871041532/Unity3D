// -----------------------------------------------------------------
// File:    AnimationEase.cs
// Author:  liuwei
// Date:    2017.02.23
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    class AnimationEase: AnimationInterval
    {
        private AnimationInterval animation;//子动画的step方法不会执行
        private float rate;

        public AnimationEase(float rate, AnimationInterval animation): base(animation.Duration)
        {
            this.animation = animation;
            this.rate = rate;
        }

        public override void Start()
        {
            base.Start();
            this.animation.Transform = this.Transform;
            this.animation.Start();
        }

        public override void Update(float ratio)
        {
            this.animation.Update(Mathf.Pow(ratio, this.rate));
        }
    }
}
