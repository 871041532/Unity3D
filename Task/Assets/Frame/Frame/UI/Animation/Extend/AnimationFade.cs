// -----------------------------------------------------------------
// File:    AnimationFade.cs
// Author:  liuwei
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    class AnimationFade : AnimationInterval
    {
        private float startAlpha = -1.0f;
        private float endAlpha;
        private List<Object> components = new List<Object>();
        private List<Color> colors = new List<Color>();

        public AnimationFade(float duration, float endAlpha) : base(duration)
        {
            this.endAlpha = endAlpha;
        }

        private void _RecoverAlpha(Transform root)
        {
            var graphic = root.GetComponent<Graphic>();
            if (null != graphic)
            {
                var color = graphic.color;
                color.a = 1;
                graphic.color = color;
            }

            var spriteRenderer = root.GetComponent<SpriteRenderer>();
            if (null != spriteRenderer)
            {
                var color = spriteRenderer.color;
                color.a = 1;
                spriteRenderer.color = color;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                _RecoverAlpha(root.GetChild(i));
            }
        }

        private void _AddGraphic(Transform root)
        {
            var graphic = root.GetComponent<Graphic>();
            if (null != graphic)
            {
                this.components.Add(graphic);
                this.colors.Add(graphic.color);
            }

            var spriteRenderer = root.GetComponent<SpriteRenderer>();
            if (null != spriteRenderer)
            {
                this.components.Add(spriteRenderer);
                this.colors.Add(spriteRenderer.color);
            }

            for (int i = 0; i < root.childCount; i++)
            {
                _AddGraphic(root.GetChild(i));
            }
        }

        public override void Start()
        {
            base.Start();
            if(this.Duration <= 0.01f)
            {
                _RecoverAlpha(this.Transform);
                return;
            }
            this.components.Clear();
            this.colors.Clear();

            var graphic = this.Transform.GetComponent<Graphic>();
            if (null != graphic)
            {
                this.startAlpha = graphic.color.a;
            }

            var spriteRenderer = this.Transform.GetComponent<SpriteRenderer>();
            if (null != spriteRenderer)
            {
                this.startAlpha = spriteRenderer.color.a;
            }
            if(this.startAlpha < 0.0f)
            {
                return;
            }

            _AddGraphic(this.Transform);
            for (int i = 0; i < this.components.Count; i++)
            {
                if (this.startAlpha >= 0.0f)
                {
                    var color = this.colors[i];
                    color.a = this.startAlpha;
                    //this.colors[i] = color;
                }
            }
        }

        public override void Update(float ratio)
        {
            if(this.startAlpha < 0.0f)
            {
                return;
            }
            if (this.components.Count == 0)
            {
                return;
            }
            for (int i = 0; i < this.components.Count; i++)
            {
                var color = this.colors[i];
                color.a = this.startAlpha * (1.0f - ratio) + this.endAlpha * ratio;
                if(this.components[i] is Graphic)
                {
                    (this.components[i] as Graphic).color = color;
                }
                else if(this.components[i] is SpriteRenderer)
                {
                    (this.components[i] as SpriteRenderer).color = color;
                }
            }
        }
    }
}
