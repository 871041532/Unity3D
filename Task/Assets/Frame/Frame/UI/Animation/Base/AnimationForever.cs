// -----------------------------------------------------------------
// File:    AnimationForever.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    class AnimationForever: AnimationBase
    {
        private AnimationBase animation = null;

        public AnimationForever(AnimationBase animation)
        {
            this.animation = animation;
        }

        public override void Start()
        {
            this.animation.Transform = this.Transform;
            this.animation.Start();
        }

        public override void Step(float delta)
        {
            if(this.animation == null)
            {
                return;
            }
            if(this.animation.IsDone())
            {
                this.animation.Start();
            }
            this.animation.Step(delta);
        }

        public override bool IsDone()
        {
            return false;
        }
    }
}
