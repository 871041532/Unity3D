// -----------------------------------------------------------------
// File:    SpriteAtlasLoader.cs
// Author:  mouguangyi
// Date:    2016.07.11
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    [Obsolete]
    class SpriteAtlasLoader : AssetLoader, ISpriteAtlasLoader, ISpriteAtlas
    {
        public SpriteAtlasLoader(AssetManager manager, string path) : base(manager, path)
        { }

        public ISpriteAtlas Load()
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.SPRITEATLAS, true);
            return this;
        }

        public void LoadAsync(Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.SPRITEATLAS, true, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(this);
            });
        }

        public Sprite GetSprite(string name)
        {
            var sprites = this.asset.Data as Sprite[];
            for (var i = 0; i < sprites.Length; ++i) {
                if (sprites[i].name == name) {
                    return sprites[i];
                }
            }

            return null;
        }
    }
}