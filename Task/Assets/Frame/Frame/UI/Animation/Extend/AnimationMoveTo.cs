// -----------------------------------------------------------------
// File:    AnimationMoveTo.cs
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
    class AnimationMoveTo: AnimationInterval
    {
        protected Vector3 startPosition;
        protected Vector3 endPosition;

        public AnimationMoveTo(float duration, Vector3 endPosition) :base(duration)
        {
            this.endPosition = endPosition;
        }

        public override void Start()
        {
            base.Start();
            this.startPosition = this.Transform.anchoredPosition;
        }

        public override void Update(float ratio)
        {
            this.Transform.anchoredPosition = Vector3.Lerp(this.startPosition, this.endPosition, ratio);
        }
    }
}
