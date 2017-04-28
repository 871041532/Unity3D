// -----------------------------------------------------------------
// File:    AnimationCallback.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    class AnimationCallback: AnimationBase
    {
        private Action callback;
        private bool isDone;

        public AnimationCallback(Action callback)
        {
            this.callback = callback;
        }

        public override void Start()
        {
            this.isDone = false;
        }

        public override void Step(float delta)
        {
            if(!this.isDone)
            {
                if(null != this.callback)
                {
                    this.callback();
                }
                this.isDone = true;
            }
        }

        public override bool IsDone()
        {
            return this.isDone;
        }
    }
}
