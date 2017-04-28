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
    /// <summary>
    /// 
    /// </summary>
    class Shake
    {
		public Vector3 V;
        public Vector3 Range;
        public float Modulus;

        public Vector3 Calculate()
        {
            var v = Vector3.zero;
			if(this.Range.x != 0.0f)
			{
                v.x = Random.Range(-this.Range.x, this.Range.x);
			}
			if(this.Range.y != 0.0f)
			{
                v.y = Random.Range(-this.Range.y, this.Range.y);
			}
			if(this.Range.z != 0.0f)
			{
                v.z = Random.Range(-this.Range.z, this.Range.z);
			}
			this.Range *= this.Modulus;
			return this.V + v;
        }
    }
}