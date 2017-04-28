// -----------------------------------------------------------------
// File:    AnimationBlink.cs
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
    class AnimationBlink : AnimationInterval
    {
        private int blinkTimes;
        private Vector3 scale;
        private bool firstInit = true;

        public AnimationBlink(float duration, int blinkTimes) : base(duration)
        {
            this.blinkTimes = blinkTimes;
        }

        public override void Start()
        {
            base.Start();
            if (this.firstInit)
            {
                this.scale = this.Transform.localScale;
                this.firstInit = false;
            }
        }

        public override void Update(float ratio)
        {
            float m = (ratio * this.blinkTimes) % 1;
            this.Transform.localScale = (m == 0 || m > 0.5f) ? this.scale : Vector3.zero;
        }
    }
}
