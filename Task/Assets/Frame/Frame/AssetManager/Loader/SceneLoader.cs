// -----------------------------------------------------------------
// File:    SceneLoader.cs
// Author:  mouguangyi
// Date:    2016.07.11
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine.SceneManagement;

namespace GameBox.Service.AssetManager
{
    [Obsolete]
    class SceneLoader : AssetLoader, ISceneLoader
    {
        public SceneLoader(AssetManager manager, string path) : base(manager, path)
        { }

        public void Load()
        {
            _LoadWithMode(LoadSceneMode.Single);
        }

        public void LoadAsync(Action<object> handler)
        {
            _LoadWithModeAsync(LoadSceneMode.Single, handler);
        }

        public void LoadAdditive()
        {
            _LoadWithMode(LoadSceneMode.Additive);
        }

        public void LoadAdditiveAsync(Action<object> handler)
        {
            _LoadWithModeAsync(LoadSceneMode.Additive, handler);
        }

        private void _LoadWithMode(LoadSceneMode mode)
        {
            this.asset = this.manager.LoadAsset(this.path, AssetType.SCENE, true, mode);
            return;
        }

        private void _LoadWithModeAsync(LoadSceneMode mode, Action<object> handler)
        {
            SetHandler(handler);

            this.manager.LoadAssetAsync(this.path, AssetType.SCENE, true, (target, message) =>
            {
                this.asset = target as Asset;
                NotifyLoaded(null);
            }, mode);
        }
    }
}