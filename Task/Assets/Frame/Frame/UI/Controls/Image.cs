// -----------------------------------------------------------------
// File:    Image.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using GameBox.Framework;
using GameBox.Service.AssetManager;
using System;

namespace GameBox.Service.UI
{
    class Image : Element, IImage
    {
        public Image(string path, RectTransform transform)
            : base(path, transform, UIType.IMAGE)
        {
            this.image = this.transform.GetComponent<UnityEngine.UI.Image>();
        }

        public void SetSprite(string path, string defaultPath = "")
        {
            if (!_SetSprite(path)) {
                _SetSprite(defaultPath);
            }
        }

        public void SetSpriteAsync(string path, string defaultPath = "", Action callback = null)
        {
            _SetSpriteAsync(path, succeed =>
            {
                if (!succeed) {
                    _SetSpriteAsync(defaultPath, null);
                }

                if (null != callback) {
                    callback();
                }
            });
        }

        public void SetSpriteAtlas(string path, string name, string defaultPath = "")
        {
            _Clear();

            if (!string.IsNullOrEmpty(path)) {
                this.asset = ServiceCenter.GetService<IAssetManager>().Load(path, AssetType.SPRITEATLAS);
                var sprite = _GetSpriteInAtlas(this.asset, name);
                if (null != sprite) {
                    this.image.sprite = sprite;
                } else {
                    _SetSprite(defaultPath);
                }
            }
        }

        public void SetSpriteAtlasAsync(string path, string name, string defaultPath = "", Action callback = null)
        {
            _Clear();

            if (!string.IsNullOrEmpty(path)) {
                this.image.enabled = false;
                ServiceCenter.GetService<IAssetManager>().LoadAsync(path, AssetType.SPRITEATLAS, asset =>
                {
                    this.asset = asset;
                    var sprite = _GetSpriteInAtlas(this.asset, name);
                    if (null != sprite) {
                        this.image.enabled = true;
                        this.image.sprite = sprite;
                    } else {
                        _SetSpriteAsync(defaultPath, null);
                    }

                    if (null != callback) {
                        callback();
                    }
                });
            }
        }

        internal override void _Reset()
        {
            this.image.enabled = true;
            _Clear();
            base._Reset();
        }

        private void _Clear()
        {
            if (null != this.asset) {
                this.asset.Dispose();
                this.asset = null;
            }
        }

        private bool _SetSprite(string path)
        {
            _Clear();

            if (!string.IsNullOrEmpty(path)) {
                this.asset = ServiceCenter.GetService<IAssetManager>().Load(path, AssetType.TEXTURE);
                var texture = this.asset.Cast<Texture2D>();
                if (null != texture) {
                    this.image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    return true;
                }
            }

            return false;
        }

        private void _SetSpriteAsync(string path, Action<bool> callback)
        {
            _Clear();

            if (!string.IsNullOrEmpty(path)) {
                this.image.enabled = false;
                ServiceCenter.GetService<IAssetManager>().LoadAsync(path, AssetType.TEXTURE, asset =>
                {
                    this.asset = asset;
                    var texture = asset.Cast<Texture2D>();
                    if (null != texture) {
                        this.image.enabled = true;
                        this.image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    }

                    if (null != callback) {
                        callback(null != texture);
                    }
                });
            }
        }

        private UnityEngine.UI.Image image = null;
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
