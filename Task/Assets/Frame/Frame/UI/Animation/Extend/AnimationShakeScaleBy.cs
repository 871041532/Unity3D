// -----------------------------------------------------------------
// File:    AnimationShakeScaleTo.cs
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
    class AnimationShakeScaleBy : AnimationInterval
    {
        protected Shake shake = new Shake();
        public AnimationShakeScaleBy(float duration, float offsetScale, float modulus) : base(duration)
        {
            this.shake.Range = new Vector3(offsetScale, offsetScale, 1);
            this.shake.Modulus = modulus;
        }

        public override void Start()
        {
            base.Start();
            this.shake.V = this.Transform.localScale;
        }

        public override void Update(float ratio)
        {
            this.Transform.localScale = this.shake.Calculate();
        }

        public override bool IsDone()
        {
            if (base.IsDone())
            {
                this.Transform.localScale = this.shake.V;
            }
            return base.IsDone();
        }
    }
}
