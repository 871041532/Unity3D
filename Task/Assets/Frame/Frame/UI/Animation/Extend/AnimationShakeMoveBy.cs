// -----------------------------------------------------------------
// File:    AnimationShakeMoveTo.cs
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
    class AnimationShakeMoveBy: AnimationInterval
    {
        protected Shake shake = new Shake();
        public AnimationShakeMoveBy(float duration, Vector3 offsetPosition, float modulus) : base(duration)
        {
            this.shake.Range = offsetPosition;
            this.shake.Modulus = modulus;
        }

        public override void Start()
        {
            base.Start();
            this.shake.V = this.Transform.anchoredPosition;
        }

        public override void Update(float ratio)
        {
            this.Transform.anchoredPosition = this.shake.Calculate();
        }

        public override bool IsDone()
        {
            if(base.IsDone())
            {
                this.Transform.anchoredPosition = this.shake.V;
            }
            return base.IsDone();
        }
    }
}
