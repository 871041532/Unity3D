// -----------------------------------------------------------------
// File:    AnimationMoveBy.cs
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
    class AnimationMoveBy: AnimationMoveTo
    {
        private Vector3 offsetPosition;

        public AnimationMoveBy(float duration, Vector3 offsetPosition) :base(duration, Vector3.zero)
        {
            this.offsetPosition = offsetPosition;
        }

        public override void Start()
        {
            base.Start();
            this.endPosition = this.startPosition + this.offsetPosition;
        }
    }
}
