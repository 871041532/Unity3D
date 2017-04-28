// -----------------------------------------------------------------
// File:    TextureLoader.cs
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
    class TextureLoader : AssetLoader, ITextureLoader
    {
        public TextureLoader(AssetManager manager, string path) : base(manager, path)
        { }

        public Texture Load()
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.TEXTURE, false);
            return (null != this.asset ? this.asset.Data as Texture : null);
        }

        public void LoadAsync(Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.TEXTURE, false, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(null != this.asset ? this.asset.Data as Texture : null);
            });
        }
    }
}