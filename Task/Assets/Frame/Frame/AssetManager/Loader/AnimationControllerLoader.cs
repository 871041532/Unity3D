// -----------------------------------------------------------------
// File:    AnimationControllerLoader.cs
// Author:  fuzhun
// Date:    2016.11.17
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    [Obsolete]
    class AnimationControllerLoader : AssetLoader, IAnimtionControllerLoader
    {
        public AnimationControllerLoader(AssetManager manager, string path)
            : base(manager, path)
        { }

        public RuntimeAnimatorController Load()
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.ANIMATIONCONTROLLER, false);
            return (null != this.asset ? this.asset.Data as RuntimeAnimatorController : null);
        }

        public void LoadAsync(Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.ANIMATIONCONTROLLER, false, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(null != this.asset ? this.asset.Data as RuntimeAnimatorController : null);
            });
        }
    }
}
