// -----------------------------------------------------------------
// File:    AnimationSequence.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    class AnimationSequence: AnimationBase
    {
        private List<AnimationBase> animations = new List<AnimationBase>();
        private int animationIndex = -1;

        public AnimationSequence(params AnimationBase[] animations)
        {
            for(int i=0;i< animations.Length;i++)
            {
                this.animations.Add(animations[i]);
            }
        }

        public AnimationSequence(List<AnimationBase> animationList, params AnimationBase[] animations)
        {
            for (int i = 0; i < animationList.Count; i++)
            {
                this.animations.Add(animationList[i]);
            }

            for (int i = 0; i < animations.Length; i++)
            {
                this.animations.Add(animations[i]);
            }
        }

        public override void Start()
        {
            this.animationIndex = -1;
            for(int i = 0; i < this.animations.Count; i++)
            {
                this.animations[i].Transform = this.Transform;
                //this.animations[i].Start();
            }
        }

        public override void Step(float delta)
        {
            if(this.animationIndex == -1)
            {
                if(this.animations.Count > 0)
                {
                    this.animationIndex = 0;
                    this.animations[this.animationIndex].Start();
                }
            }
            else
            {
                if(!this.animations[this.animationIndex].IsDone())
                {
                    this.animations[this.animationIndex].Step(delta);
                }
                else if(this.animationIndex < this.animations.Count - 1)
                {
                    this.animationIndex++;
                    this.animations[this.animationIndex].Start();
                }
            }
        }

        public override bool IsDone()
        {
            for (int i = 0; i < this.animations.Count; i++)
            {
                if(!this.animations[i].IsDone())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
