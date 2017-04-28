// -----------------------------------------------------------------
// File:    Asset.cs
// Author:  mouguangyi
// Date:    2016.07.07
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameBox.Service.AssetManager
{
    class Asset : CRef<Asset>
    {
        public Asset(string path, AssetType type, AssetPack pack)
        {
            this.path = path;
            this.type = type;
            this.pack = pack;
            this.dispatcher = new Dispatcher(this);
        }

        public void TrackDispose(Action<object, Message> handler)
        {
            this.dispatcher.AddListener(Message.DESTROY, handler);
        }

        public Asset Get(params object[] more)
        {
            if (null == this.data) {
                if (AssetType.SCENE == this.type) {
                    var realPath = System.IO.Path.GetFileNameWithoutExtension(this.path);
                    var mode = (LoadSceneMode)more[0];
                    SceneManager.LoadScene(realPath, mode);
                    this.data = SceneManager.GetSceneByName(this.path);
                } else if (AssetPackLoadType.RESOURCES == this.pack.LoadType) {
                    _ProcessResourcesOrigin();
                } else if (AssetPackType.FLAT == this.pack.Type) {
                    _ProcessWWWOrigin();
                } else if (AssetPackType.BUNDLE == this.pack.Type) {
                    _ProcessBundleAsset();
                }
            }

            return this;
        }

        public void GetAsync(Action<object, Message> callback, params object[] more)
        {
            this.dispatcher.AddListener(Message.COMPLETED, callback);

            if (null != this.data) {
                _NotifyCallback();
            } else if (AssetType.SCENE == this.type) {
                var realPath = System.IO.Path.GetFileNameWithoutExtension(this.path);
                var mode = (LoadSceneMode)more[0];
                new SceneLoadTask(realPath, mode).Start().Continue(task =>
                {
                    this.data = SceneManager.GetSceneByName(this.path);
                    _NotifyCallback();
                    return null;
                });
            } else if (AssetPackLoadType.RESOURCES == this.pack.LoadType) {
                _ProcessResourcesOriginAsync();
            } else if (AssetPackType.FLAT == this.pack.Type) {
                _ProcessWWWOriginAsync();
            } else if (AssetPackType.BUNDLE == this.pack.Type) {
                _ProcessBundleAssetAsync();
            }
        }

        public string Path
        {
            get {
                return this.path;
            }
        }

        public object Data
        {
            get {
                return this.data;
            }
        }

        protected override void Dispose()
        {
            this.dispatcher.Notify(Message.Destroy);
            this.dispatcher = null;

            if (AssetType.SCENE == this.type && null != this.data) {
                SceneManager.UnloadScene(this.path);
            }

            base.Dispose();
        }

        private void _ProcessResourcesOrigin()
        {
            switch (this.type) {
            case AssetType.TEXT:
                this.data = (this.pack.Origin as TextAsset).text;
                break;
            case AssetType.BYTES:
                this.data = (this.pack.Origin as TextAsset).bytes;
                break;
            default:
                this.data = this.pack.Origin;
                break;
            }
        }

        private void _ProcessResourcesOriginAsync()
        {
            _ProcessResourcesOrigin();
            _NotifyCallback();
        }

        private void _ProcessWWWOrigin()
        {
            var w = this.pack.Origin as WWW;
            switch (this.type) {
            case AssetType.TEXT:
                this.data = w.text;
                break;
            case AssetType.TEXTURE:
                this.data = w.texture;
                break;
            case AssetType.SPRITEATLAS:
                var bundle = w.assetBundle;
                this.data = bundle.LoadAllAssets<Sprite>();
                break;
            case AssetType.BYTES:
            default:
                this.data = w.bytes;
                break;
            }
        }

        private void _ProcessWWWOriginAsync()
        {
            var w = this.pack.Origin as WWW;
            switch (this.type) {
            case AssetType.TEXT:
                this.data = w.text;
                _NotifyCallback();
                break;
            case AssetType.TEXTURE:
                this.data = w.texture;
                _NotifyCallback();
                break;
            case AssetType.SPRITEATLAS:
                var bundle = w.assetBundle;
                new AssetsLoadFromBundleTask<Sprite>(bundle).Start().Continue(task =>
                {
                    var request = task.Result as AssetBundleRequest;
                    this.data = Array.ConvertAll<object, Sprite>(request.allAssets, o => (Sprite)o);
                    _NotifyCallback();
                    return null;
                });
                break;
            case AssetType.BYTES:
            default:
                this.data = w.bytes;
                _NotifyCallback();
                break;
            }
        }

        private void _ProcessBundleAsset()
        {
            var bundle = this.pack.Origin as AssetBundle;
            if (AssetType.SPRITEATLAS == this.type) {
                this.data = bundle.LoadAllAssets<Sprite>();
            } else {
                var realPath = AssetType.UNKNOWN != this.type ? ("assets/resources/" + this.path).ToLower() : this.path;
                this.data = bundle.LoadAsset(realPath);
                switch (this.type) {
                case AssetType.TEXT:
                    this.data = (this.data as TextAsset).text;
                    break;
                case AssetType.BYTES:
                    this.data = (this.data as TextAsset).bytes;
                    break;
                }
            }
        }

        private void _ProcessBundleAssetAsync()
        {
            var bundle = this.pack.Origin as AssetBundle;
            if (AssetType.SPRITEATLAS == this.type) {
                new AssetsLoadFromBundleTask<Sprite>(bundle).Start().Continue(task =>
                {
                    var request = task.Result as AssetBundleRequest;
                    this.data = Array.ConvertAll<object, Sprite>(request.allAssets, o => (Sprite)o);
                    _NotifyCallback();
                    return null;
                });

            } else {
                var realPath = AssetType.UNKNOWN != this.type ? ("assets/resources/" + this.path).ToLower() : this.path;
                new AssetLoadFromBundleTask(bundle, realPath).Start().Continue(task =>
                {
                    var request = task.Result as AssetBundleRequest;
                    this.data = request.asset;
                    switch (this.type) {
                    case AssetType.TEXT:
                        this.data = (this.data as TextAsset).text;
                        break;
                    case AssetType.BYTES:
                        this.data = (this.data as TextAsset).bytes;
                        break;
                    }
                    _NotifyCallback();
                    return null;
                });
            }
        }

        private void _NotifyCallback()
        {
            new CompletedTask().Start().Continue(task =>
            {
                this.dispatcher.Notify(Message.Completed, true);
                return null;
            });
        }

        private string path = null;
        private AssetType type = AssetType.UNKNOWN;
        private AssetPack pack = null;
        private object data = null;
        private Dispatcher dispatcher = null;
    }
}