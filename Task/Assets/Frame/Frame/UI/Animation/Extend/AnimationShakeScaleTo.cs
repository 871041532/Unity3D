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
    class AnimationShakeScaleTo : AnimationShakeScaleBy
    {
        public AnimationShakeScaleTo(float duration, float scale, float modulus) : base(duration, scale, modulus)
        {
            
        }

        public override void Start()
        {
            base.Start();
            this.shake.Range = this.shake.Range - this.shake.V;
        }
    }
}
