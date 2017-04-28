// -----------------------------------------------------------------
// File:    AnimationBezierScaleBy.cs
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
    class AnimationBezierScaleBy: AnimationBezierScaleTo
    {
        public AnimationBezierScaleBy(float duration, float offsetMiddle, float offsetEnd) : base(duration, offsetMiddle, offsetEnd)
        {
        }

        public AnimationBezierScaleBy(float duration, float offsetMiddle1, float offsetMiddle2, float offsetEnd) : base(duration, offsetMiddle1, offsetMiddle2, offsetEnd)
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
