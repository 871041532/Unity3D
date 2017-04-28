// -----------------------------------------------------------------
// File:    AudioClipLoader.cs
// Author:  fuzhun
// Date:    2016.09.18
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    [Obsolete]
    class AudioClipLoader : AssetLoader, IAudioClipLoader
    {
        public AudioClipLoader(AssetManager manager, string path) : base(manager, path)
        { }

        public AudioClip Load()
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.AUDIOCLIP, false);
            return (null != this.asset ? this.asset.Data as AudioClip : null);
        }

        public void LoadAsync(Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.AUDIOCLIP, false, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(null != this.asset ? this.asset.Data as AudioClip : null);
            });
        }
    }
}
