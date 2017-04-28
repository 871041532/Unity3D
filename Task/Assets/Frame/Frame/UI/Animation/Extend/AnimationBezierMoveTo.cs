// -----------------------------------------------------------------
// File:    AnimationBezierMoveTo.cs
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
    class AnimationBezierMoveTo: AnimationInterval
    {
        protected Bezier bezier = new Bezier();

        public AnimationBezierMoveTo(float duration, Vector3 positionMiddle, Vector3 positionEnd) : base(duration)
        {
            this.bezier.BezierType = BezierType.Bezier2;
            this.bezier.V1 = positionMiddle;
            this.bezier.V2 = positionEnd;
        }

        public AnimationBezierMoveTo(float duration, Vector3 positionMiddle1, Vector3 positionMiddle2, Vector3 positionEnd) : base(duration)
        {
            this.bezier.BezierType = BezierType.Bezier3;
            this.bezier.V1 = positionMiddle1;
            this.bezier.V2 = positionMiddle2;
            this.bezier.V3 = positionEnd;
        }

        public override void Start()
        {
            base.Start();
            this.bezier.V0 = this.Transform.anchoredPosition;
        }

        public override void Update(float ratio)
        {
            this.Transform.anchoredPosition = this.bezier.Calculate(ratio);
        }
    }
}
