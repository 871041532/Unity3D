// -----------------------------------------------------------------
// File:    ImageAnimation.cs
// Author:  liuwei
// Date:    2017.02.06
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using GameBox.Service.AssetManager;
using System.Collections.Generic;
using GameBox.Framework;

public struct SpriteLoadingInformation
{
    public string path;
    public string name;
}

namespace GameBox.Service.UI
{
    class ImageAnimation : Element, IImageAnimation
    {
        public ImageAnimation(string path, RectTransform transform)
            : base(path, transform, UIType.IMAGEANIMATION)
        {
            this.imageAnimation = this.transform.GetComponent<Extension.ImageAnimation>();
        }

        public void Play()
        {
            imageAnimation.Play();
        }

        public void Pause()
        {
            imageAnimation.Pause();
        }

        public void SetSpriteAnimation(List<SpriteLoadingInformation> spriteLoadingInformatonList, int framesPerSecond = 10)
        {
            imageAnimation = this.transform.GetComponent<Extension.ImageAnimation>();
            if (imageAnimation == null) {
                imageAnimation = this.transform.gameObject.AddComponent<Extension.ImageAnimation>();
            }

            imageAnimation.FramesPerSecond = framesPerSecond;
            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < spriteLoadingInformatonList.Count; i++)
            {
                var spriteLoadingInformaton = spriteLoadingInformatonList[i];
                var sprite = _LoadSpriteFromAtlas(spriteLoadingInformaton.path, spriteLoadingInformaton.name);
                sprites.Add(sprite);
            }
            imageAnimation.Sprites = sprites;
        }

        private Sprite _LoadSpriteFromAtlas(string spritePath, string spriteName)
        {
            this.asset = ServiceCenter.GetService<IAssetManager>().Load(spritePath, AssetType.SPRITEATLAS);
            return _GetSpriteInAtlas(this.asset, spriteName);
        }

        internal override void _Reset()
        {
            this.imageAnimation.enabled = true;
            _Clear();
            base._Reset();
        }

        private void _Clear()
        {
            if (null != this.asset)
            {
                this.asset.Dispose();
                this.asset = null;
            }
        }

        private Extension.ImageAnimation imageAnimation = null;
        private IAsset asset = null;

        private static Sprite _GetSpriteInAtlas(IAsset asset, string name)
        {
            if (null == asset) {
                return null;
            }

            var sprites = asset.Cast<Sprite[]>();
            for (var i = 0; i < sprites.Length; ++i) {
                if (sprites[i].name == name) {
                    return sprites[i];
                }
            }

            return null;
        }
    }
}
