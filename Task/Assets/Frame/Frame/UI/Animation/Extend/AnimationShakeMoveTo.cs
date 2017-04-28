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
    class AnimationShakeMoveTo: AnimationShakeMoveBy
    {
        public AnimationShakeMoveTo(float duration, Vector3 endPosition, float modulus) : base(duration, endPosition, modulus)
        {

        }

        public override void Start()
        {
            base.Start();
            this.shake.Range = this.shake.Range - this.shake.V;
        }
    }
}
