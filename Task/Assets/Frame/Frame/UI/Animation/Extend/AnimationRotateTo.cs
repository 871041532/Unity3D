// -----------------------------------------------------------------
// File:    AnimationRotateTo.cs
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
    class AnimationRotateTo: AnimationInterval
    {
        protected Vector3 startEulerAngles;
        protected Vector3 endEulerAngles;
        public AnimationRotateTo(float duration, Vector3 endEulerAngles) :base(duration)
        {
            this.endEulerAngles = endEulerAngles;
        }

        public override void Start()
        {
            base.Start();
            this.startEulerAngles = this.Transform.localEulerAngles;
        }

        public override void Update(float ratio)
        {
            this.Transform.localEulerAngles = Vector3.Lerp(this.startEulerAngles, this.endEulerAngles, ratio);
        }
    }
}
