// -----------------------------------------------------------------
// File:    AnimationInterval.cs
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
    public class AnimationInterval: AnimationBase
    {
        public float Duration = 1.0f;
        public float Elapsed = 0.0f;

        public AnimationInterval(float duration)
        {
            this.Duration = duration;
        }

        public override void Start()
        {
            this.Elapsed = 0.0f;
        }

        public override void Step(float delta)
        {
            this.Elapsed += delta;
            var ratio = this.Duration <= 0.0f ? 1.0f : (this.Elapsed / this.Duration);
            ratio = Mathf.Clamp01(ratio);
            Update(ratio);
        }

        public override bool IsDone()
        {
            return (this.Elapsed >= this.Duration);
        }

        public virtual void Update(float ratio)
        {

        }
    }
}
