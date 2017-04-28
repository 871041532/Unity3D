// -----------------------------------------------------------------
// File:    TextLoader.cs
// Author:  mouguangyi
// Date:    2016.07.11
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    [Obsolete]
    class TextLoader : AssetLoader, ITextLoader
    {
        public TextLoader(AssetManager manager, string path) : base(manager, path)
        { }

        public string Load()
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.TEXT, false);
            return (null != this.asset ? this.asset.Data as string : "");
        }

        public void LoadAsync(Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.TEXT, false, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(null != this.asset ? this.asset.Data as string : "");
            });
        }
    }
}