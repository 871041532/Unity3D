// -----------------------------------------------------------------
// File:    AnimationBase.cs
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
    public class AnimationBase
    {
        /// <summary>
        /// 
        /// </summary>
        public RectTransform Transform;
		
		public virtual void Start()
		{
			
		}
		
		public virtual void Step(float delta)
        {

        }

        public virtual bool IsDone()
        {
            return true;
        }
    }
}
