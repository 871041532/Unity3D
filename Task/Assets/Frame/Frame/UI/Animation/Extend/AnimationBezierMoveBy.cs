// -----------------------------------------------------------------
// File:    AnimationBezierMoveBy.cs
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
    class AnimationBezierMoveBy: AnimationBezierMoveTo
    {
        public AnimationBezierMoveBy(float duration, Vector3 offsetMiddle, Vector3 offsetEnd) : base(duration, offsetMiddle, offsetEnd)
        {
        }

        public AnimationBezierMoveBy(float duration, Vector3 offsetMiddle1, Vector3 offsetMiddle2, Vector3 offsetEnd) : base(duration, offsetMiddle1, offsetMiddle2, offsetEnd)
        {
        }

        public override void Start()
        {
            base.Start();
            this.bezier.V1 += this.bezier.V0;
            this.bezier.V2 += this.bezier.V0;
            if(this.bezier.BezierType == BezierType.Bezier3)
            {
                this.bezier.V3 += this.bezier.V0;
            }
        }
    }
}
