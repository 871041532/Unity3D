// -----------------------------------------------------------------
// File:    AnimationValueTo.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class AnimationValueTo: AnimationInterval
    {
        /// <summary>
        /// 
        /// </summary>
        private float from;
        private float to;
        private Action<float> callback;

        public AnimationValueTo(float duration, float from, float to, Action<float> callback):base(duration)
        {
            this.from = from;
            this.to = to;
            this.callback = callback;
        }

        public override void Update(float ratio)
        {
            if(null != this.callback)
            {
                this.callback(this.from * (1.0f - ratio) + this.to * ratio);
            }
        }
    }
}
