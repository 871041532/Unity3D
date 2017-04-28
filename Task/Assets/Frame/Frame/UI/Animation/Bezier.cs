// -----------------------------------------------------------------
// File:    AnimationFadeOut.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------

using UnityEngine;

namespace GameBox.Service.UI
{
    enum BezierType
    {
        Bezier2 = 0,
        Bezier3 = 1,
    }
    /// <summary>
    /// 
    /// </summary>
    class Bezier
    {
        public BezierType BezierType;
        public Vector3 V0;//起始点
        public Vector3 V1;
        public Vector3 V2;
        public Vector3 V3;

        public Vector3 Calculate(float ratio)
        {
            if(this.BezierType == BezierType.Bezier2)
            {
                return this.V0 * (1 - ratio) * (1 - ratio) + this.V1 * 2 * ratio * (1 - ratio) + this.V2 * ratio * ratio;
            }
            else
            {
                return this.V0 * (1 - ratio) * (1 - ratio) * (1 - ratio) + this.V1 * 3 * ratio * (1 - ratio) * (1 - ratio) + this.V2 * 3 * ratio * ratio * (1 - ratio) + this.V3 * ratio * ratio * ratio;
            }
        }
    }
}