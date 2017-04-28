// -----------------------------------------------------------------
// File:    PrefabLoader.cs
// Author:  mouguangyi
// Date:    2016.07.11
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    [Obsolete]
    class PrefabLoader : AssetLoader, IPrefabLoader
    {
        public PrefabLoader(AssetManager manager, string path) : base(manager, path)
        { }

        public UnityEngine.Object Load()
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.PREFAB, true);
            return this.asset.Data as UnityEngine.Object;
        }

        public void LoadAsync(Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.PREFAB, true, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(this.asset.Data);
            });
        }
    }
}