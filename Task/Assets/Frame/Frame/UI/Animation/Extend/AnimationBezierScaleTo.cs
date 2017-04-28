// -----------------------------------------------------------------
// File:    AnimationBezierScaleTo.cs
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
    class AnimationBezierScaleTo : AnimationInterval
    {
        protected Bezier bezier = new Bezier();

        public AnimationBezierScaleTo(float duration, float scaleMiddle, float scaleEnd) : base(duration)
        {
            this.bezier.BezierType = BezierType.Bezier2;
            this.bezier.V1 = new Vector3(scaleMiddle, scaleMiddle, scaleMiddle);
            this.bezier.V2 = new Vector3(scaleEnd, scaleEnd, scaleEnd);
        }

        public AnimationBezierScaleTo(float duration, float scaleMiddle1, float scaleMiddle2, float scaleEnd) : base(duration)
        {
            this.bezier.BezierType = BezierType.Bezier3;
            this.bezier.V1 = new Vector3(scaleMiddle1, scaleMiddle1, scaleMiddle1);
            this.bezier.V2 = new Vector3(scaleMiddle2, scaleMiddle2, scaleMiddle2);
            this.bezier.V3 = new Vector3(scaleEnd, scaleEnd, scaleEnd);
        }

        public override void Start()
        {
            base.Start();
            this.bezier.V0 = this.Transform.localScale;
        }

        public override void Update(float ratio)
        {
            this.Transform.localScale = this.bezier.Calculate(ratio);
        }
    }
}
