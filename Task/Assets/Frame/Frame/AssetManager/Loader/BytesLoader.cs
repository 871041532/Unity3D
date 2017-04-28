// -----------------------------------------------------------------
// File:    BytesLoader.cs
// Author:  mouguangyi
// Date:    2016.07.11
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    [Obsolete]
    class BytesLoader : AssetLoader, IBytesLoader
    {
        public BytesLoader(AssetManager manager, string path) : base(manager, path)
        { }

        public byte[] Load()
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.BYTES, true);
            return (null != this.asset ? this.asset.Data as byte[] : null);
        }

        public void LoadAsync(Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.BYTES, true, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(null != this.asset ? this.asset.Data as byte[] : null);
            });
        }
    }
}