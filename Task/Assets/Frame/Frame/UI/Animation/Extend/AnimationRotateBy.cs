// -----------------------------------------------------------------
// File:    AnimationRotateBy.cs
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
    class AnimationRotateBy: AnimationRotateTo
    {
        private Vector3 ratioEulerAngles;

        public AnimationRotateBy(float duration, Vector3 ratioEulerAngles) :base(duration, Vector3.zero)
        {
            this.ratioEulerAngles = ratioEulerAngles;
        }

        public override void Start()
        {
            base.Start();
            this.endEulerAngles = new Vector3(this.endEulerAngles.x * this.ratioEulerAngles.x, this.endEulerAngles.y * this.ratioEulerAngles.y, this.endEulerAngles.z * this.ratioEulerAngles.z);
        }
    }
}
