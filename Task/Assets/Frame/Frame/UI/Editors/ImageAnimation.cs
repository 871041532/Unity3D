// -----------------------------------------------------------------
// File:    ImageAnimation.cs
// Author:  liuwei
// Date:    2017.02.04
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace GameBox.Service.UI.Extension
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class ImageAnimation : MonoBehaviour
    {
        public List<Sprite> Sprites = new List<Sprite>();
        public int FramesPerSecond;
        public bool Snap = false;
        public bool Loop = true;

        void Start()
        {
            if (image == null)
            {
                image = GetComponent<UnityEngine.UI.Image>();
            }
        }

        /// <summary>
        /// Advance the sprite animation process.
        /// </summary>
        protected virtual void Update()
        {
            if (isPlaying && Sprites.Count > 1 && Application.isPlaying && FramesPerSecond > 0)
            {
                delta += Time.deltaTime;
                float rate = 1f / FramesPerSecond;

                if (rate < delta)
                {
                    delta = (rate > 0f) ? delta - rate : 0f;

                    if (++index >= Sprites.Count)
                    {
                        index = 0;
                        isPlaying = Loop;
                    }

                    if (isPlaying)
                    {
                        image.sprite = Sprites[index];
                        if (Snap) image.SetNativeSize();
                    }
                }
            }
        }

        public void Play()
        {
            isPlaying = true;
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void ResetToBeginning()
        {
            isPlaying = true;
            index = 0;

            if (image != null && Sprites.Count > 0)
            {
                image.sprite = Sprites[index];
                if (Snap) image.SetNativeSize();
            }
        }

        private UnityEngine.UI.Image image;
        private float delta = 0f;
        private int index = 0;
        private bool isPlaying = true;
    }
}

