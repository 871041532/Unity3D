// -----------------------------------------------------------------
// File:    AnimationSpawn.cs
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
    class AnimationSpawn: AnimationBase
    {
        private List<AnimationBase> animations = new List<AnimationBase>();

        public AnimationSpawn(params AnimationBase[] animations)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                this.animations.Add(animations[i]);
            }
        }

        public AnimationSpawn(List<AnimationBase> animationList, params AnimationBase[] animations)
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
            for (int i = 0; i < this.animations.Count; i++)
            {
                this.animations[i].Transform = this.Transform;
                this.animations[i].Start();
            }
        }

        public override void Step(float delta)
        {
            for (int i = 0; i < this.animations.Count; i++)
            {
                if (!this.animations[i].IsDone())
                {
                    this.animations[i].Step(delta);
                }
            }
        }

        public override bool IsDone()
        {
            for (int i = 0; i < this.animations.Count; i++)
            {
                if (!this.animations[i].IsDone())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
