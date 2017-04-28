// -----------------------------------------------------------------
// File:    AssetPack.cs
// Author:  mouguangyi
// Date:    2016.07.07
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameBox.Service.AssetManager
{
    class AssetPack : CRef<AssetPack>
    {
        public AssetPack(AssetManager manager, string path, AssetPackType type, AssetPackLoadType loadType, bool checkDependency)
        {
            this.manager = manager;
            this.path = path;
            this.type = type;
            this.loadType = loadType;
            this.checkDependency = checkDependency;
            this.dispatcher = new Dispatcher(this);
            this.lifeTime = LIFE_INITIAL;
            Logger<IAssetManager>.L(this.path + " is created.");
        }

        public void Load()
        {
            if (AssetPackState.INVALID == this.state) {
                this.state = AssetPackState.LOADING;
                string[] dependencies = null;
                if (this.checkDependency && null != (dependencies = this.manager.GetDependence(this.path))) {
                    for (var i = 0; i < dependencies.Length; ++i) {
                        var pack = this.manager.LoadAssetPack(dependencies[i], true);
                        if (null != pack) {
                            this.dependPacks.Add(pack);
                        } else {
                            Logger<IAssetManager>.E("Load asset pack (" + dependencies[i] + ") failed!");
                            this.state = AssetPackState.FAILED;
                            return;
                        }
                    }
                }

                this.origin = _LoadSelf();
                this.state = (null != this.origin ? AssetPackState.LOADED : AssetPackState.FAILED);
            }
        }

        public void LoadAsync(Action<object, Message> callback)
        {
            this.dispatcher.AddListener(Message.COMPLETED, callback);

            switch (this.state) {
            case AssetPackState.INVALID:
                this.state = AssetPackState.LOADING;
                string[] dependencies = null;
                if (this.checkDependency && null != (dependencies = this.manager.GetDependence(this.path))) {
                    for (var i = 0; i < dependencies.Length; ++i) {
                        var dependency = dependencies[i];
                        this.manager.LoadAssetPackAsync(dependency, true, (target, _) =>
                        {
                            var pack = target as AssetPack;
                            if (null != pack) {
                                this.dependPacks.Add(pack);

                                if (this.dependPacks.Count == dependencies.Length) {
                                    _LoadSelfAsync();
                                }
                            } else {
                                Logger<IAssetManager>.E("Load asset pack (" + dependency + ") failed!");
                                this.state = AssetPackState.FAILED;
                            }
                        });
                    }
                } else {
                    _LoadSelfAsync();
                }
                break;
            case AssetPackState.LOADED:
                _OnAssetPackLoaded();
                break;
            }
        }

        public Asset GetAsset(string id, AssetType type, params object[] more)
        {
            if (AssetPackState.INVALID == this.state || AssetPackState.LOADING == this.state) {
                throw new InvalidOperationException("Asset Pack has not been loaded!");
            }

            if (AssetPackState.FAILED == this.state) {
                return null;
            }

            Asset asset = null;
            if (!this.assets.TryGetValue(id, out asset)) {
                asset = _NewAsset(id, type);
                Retain();   // Add self ref count when create a new asset
            }

            this.lifeTime += LIFE_INCREMENT;
            asset.Retain();
            return asset.Get(more);
        }

        public void GetAssetAsync(string id, AssetType type, Action<object, Message> callback, params object[] more)
        {
            if (AssetPackState.INVALID == this.state || AssetPackState.LOADING == this.state) {
                throw new InvalidOperationException("Asset Pack has not been loaded!");
            }

            if (AssetPackState.FAILED == this.state) {
                callback(null, Message.Any);
                return;
            }

            Asset asset = null;
            if (!this.assets.TryGetValue(id, out asset)) {
                asset = _NewAsset(id, type);
                Retain();   // Add self ref count when create a new asset
            }

            this.lifeTime += LIFE_INCREMENT;
            asset.Retain();
            asset.GetAsync(callback, more);
        }

        public string Path
        {
            get {
                return this.path;
            }
        }

        public AssetPackType Type
        {
            get {
                return this.type;
            }
        }

        public AssetPackLoadType LoadType
        {
            get {
                return this.loadType;
            }
            set {
                this.loadType = value;
            }
        }

        public object Origin
        {
            get {
                return this.origin;
            }
        }

        public AssetPackState State
        {
            get {
                return this.state;
            }
        }

        public float LifeTime
        {
            get {
                return this.lifeTime;
            }
            set {
                this.lifeTime = value;
            }
        }

        internal void _Reset(bool unloadAllLoadedObjects)
        {
            if (null != this.origin) {
                if (AssetPackLoadType.RESOURCES == this.loadType) {
                    if (!(this.origin is GameObject) && !(this.origin is Component)) {
                        Resources.UnloadAsset(this.origin as UnityEngine.Object);
                    }
                    Resources.UnloadUnusedAssets();
                } else if (AssetPackType.FLAT == this.type) {
                    var w = this.origin as WWW;
                    if (null != w.assetBundle) {
                        w.assetBundle.Unload(unloadAllLoadedObjects);
                    }
                    w.Dispose();
                } else {
                    var bundle = this.origin as AssetBundle;
                    bundle.Unload(unloadAllLoadedObjects);
                }

                this.origin = null;
            }

            for (var i = 0; i < this.dependPacks.Count; ++i) {
                this.dependPacks[i].Release();
            }
            this.dependPacks.Clear();

            this.state = AssetPackState.INVALID;
        }

        protected override void Dispose()
        {
            _Reset(true);

            this.dispatcher = null;
            this.dependPacks = null;
            this.manager = null;

            Logger<IAssetManager>.L(this.path + " is destroyed.");

            base.Dispose();
        }

        private object _LoadSelf()
        {
            if (AssetPackLoadType.INTERNALSCENE == this.loadType) {
                _LoadSelfFromInternalScene();
            } else if (AssetPackLoadType.RESOURCES == this.loadType) {
                return _LoadSelfFromResources();
            } else if (AssetPackType.BUNDLE == this.type) {
                return _LoadSelfFromBundle();
            } else if (AssetPackType.FLAT == this.type) {
                return _LoadSelfFromWWW();
            }

            return null;
        }

        private void _LoadSelfAsync()
        {
            if (AssetPackLoadType.INTERNALSCENE == this.loadType) {
                _LoadSelfFromInternalSceneAsync();
            } else if (AssetPackLoadType.RESOURCES == this.loadType) {
                _LoadSelfFromResourcesAsync();
            } else if (AssetPackType.BUNDLE == this.type) {
                _LoadSelfFromBundleAsync();
            } else if (AssetPackType.FLAT == this.type) {
                _LoadSelfFromWWWAsync();
            }
        }

        private void _LoadSelfFromInternalScene()
        {
            // DO NOTHING, but only save the current scene
            this.origin = SceneManager.GetActiveScene();
        }

        private void _LoadSelfFromInternalSceneAsync()
        {
            _LoadSelfFromInternalScene();
            this.state = (null != this.origin ? AssetPackState.LOADED : AssetPackState.FAILED);
            _OnAssetPackLoaded();
        }

        private object _LoadSelfFromResources()
        {
            var fullPath = _GetPathUnderResources();
            Logger<IAssetManager>.L("Load asset pack from resources: " + fullPath);
            if (AssetPackType.FLAT == this.type) {
                return Resources.Load(fullPath);
            } else {
                return Resources.LoadAll<Sprite>(fullPath);
            }
        }

        private void _LoadSelfFromResourcesAsync()
        {
            var fullPath = _GetPathUnderResources();
            Logger<IAssetManager>.L("Load asset pack from resources: " + fullPath);
            if (AssetPackType.FLAT == this.type) {
                new ResourceLoadTask(fullPath).Start().Continue(task =>
                {
                    var request = task.Result as ResourceRequest;
                    this.origin = request.asset;
                    this.state = (null != this.origin ? AssetPackState.LOADED : AssetPackState.FAILED);
                    _OnAssetPackLoaded();
                    return null;
                });
            } else {
                new ResourceLoadAllTask<Sprite>(fullPath).Start().Continue(task =>
                {
                    this.origin = task.Result;
                    this.state = (null != this.origin ? AssetPackState.LOADED : AssetPackState.FAILED);
                    _OnAssetPackLoaded();
                    return null;
                });
            }
        }

        private object _LoadSelfFromBundle()
        {
            var fullPath = "";
            switch (this.loadType) {
            case AssetPackLoadType.STREAMINGASSETS:
                fullPath = PathUtility.ComposeAppPath(this.path);
                break;
            case AssetPackLoadType.PERSISTENTDATAPATH:
                fullPath = PathUtility.ComposeDataPath(this.path);
                if (!File.Exists(fullPath)) {
                    fullPath = PathUtility.ComposeAppPath(this.path);
                    this.loadType = AssetPackLoadType.STREAMINGASSETS;
                }
                break;
            }
            Logger<IAssetManager>.L("Load asset pack through assets bundle: " + fullPath);
            return AssetBundle.LoadFromFile(fullPath);
        }

        private void _LoadSelfFromBundleAsync()
        {
            var fullPath = "";
            switch (this.loadType) {
            case AssetPackLoadType.STREAMINGASSETS:
                fullPath = PathUtility.ComposeAppPath(this.path);
                break;
            case AssetPackLoadType.PERSISTENTDATAPATH:
                fullPath = PathUtility.ComposeDataPath(this.path);
                if (!File.Exists(fullPath)) {
                    fullPath = PathUtility.ComposeAppPath(this.path);
                    this.loadType = AssetPackLoadType.STREAMINGASSETS;
                }
                break;
            }

            Logger<IAssetManager>.L("Load asset pack through assets bundle: " + fullPath);
            new AssetBundleLoadTask(fullPath).Start().Continue(task =>
            {
                var request = task.Result as AssetBundleCreateRequest;
                this.origin = request.assetBundle;
                this.state = (null != this.origin ? AssetPackState.LOADED : AssetPackState.FAILED);
                _OnAssetPackLoaded();
                return null;
            });
        }

        private object _LoadSelfFromWWW()
        {
            Logger<IAssetManager>.E("CAN NOT load unbundled file sync! Please use async load.");
            return null;
        }

        private void _LoadSelfFromWWWAsync()
        {
            var fullPath = "";
            switch (this.loadType) {
            case AssetPackLoadType.STREAMINGASSETS:
                fullPath = PathUtility.ComposeAppUrl(this.path);
                break;
            case AssetPackLoadType.PERSISTENTDATAPATH:
                fullPath = PathUtility.ComposeDataUrl(this.path);
                break;
            case AssetPackLoadType.REMOTE:
                fullPath = this.path;
                break;
            }

            Logger<IAssetManager>.L("Load asset pack through WWW: " + fullPath);
            new WWWLoadTask(fullPath).Start().Continue(task =>
            {
                var w = task.Result as WWW;
                if (null == w.error) {
                    this.origin = w;
                    this.state = AssetPackState.LOADED;
                } else {
                    this.state = AssetPackState.FAILED;
                }

                _OnAssetPackLoaded();
                return null;
            });
        }

        private void _OnAssetPackLoaded()
        {
            new CompletedTask().Start().Continue(task =>
            {
                if (null != this.dispatcher) {
                    this.dispatcher.Notify(Message.Completed, true);
                }
                return null;
            });
        }

        private Asset _NewAsset(string id, AssetType type)
        {
            var asset = new Asset(id, type, this);
            asset.TrackDispose(_OnAssetDispose);
            this.assets.Add(id, asset);

            return asset;
        }

        private void _OnAssetDispose(object target, Message message)
        {
            if (Message.DESTROY == message.Type) {
                var asset = target as Asset;
                this.assets.Remove(asset.Path);

                Release();  // Decrease self ref count when an asset dispose
            }
        }

        private string _GetPathUnderResources()
        {
            var resourcePath = this.path;
            var dotIndex = resourcePath.LastIndexOf(".");
            if (dotIndex >= 0) {
                resourcePath = resourcePath.Substring(0, dotIndex);
            }

            return resourcePath;
        }

        private AssetManager manager = null;
        private string path = null;
        private AssetPackType type = AssetPackType.FLAT;
        private AssetPackLoadType loadType = AssetPackLoadType.RESOURCES;
        private bool checkDependency = true;
        private AssetPackState state = AssetPackState.INVALID;
        private Dispatcher dispatcher = null;
        private object origin = null;
        private List<AssetPack> dependPacks = new List<AssetPack>();
        private Dictionary<string, Asset> assets = new Dictionary<string, Asset>();
        private float lifeTime = 0f;

        private const float LIFE_INITIAL = 3f;      // 3 seconds
        private const float LIFE_INCREMENT = 1f;    // 1 second

        // ---------------------------------------------------------
        // Graph
        public float DrawGraph(float yOffset)
        {
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, PACK_HEIGHT), GraphStyle.GreenTexture);
            GUI.DrawTexture(new Rect(GraphStyle.ServiceWidth - this.lifeTime - 1, yOffset + 1, this.lifeTime, PACK_HEIGHT - 1), GraphStyle.RedTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, PACK_HEIGHT), "[" + this.loadType.ToString() + "] " + this.path + ": " + this.RefCount, GraphStyle.SmallLabel);
            Asset asset = null;
            yOffset += PACK_HEIGHT + MARGIN;     
            foreach (var pair in this.assets) {
                asset = pair.Value;
                GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, ASSET_HEIGHT), GraphStyle.RedTexture);
                GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, ASSET_HEIGHT), asset.Path + ": " + asset.RefCount, GraphStyle.MiniLabel);
                yOffset += ASSET_HEIGHT + MARGIN;
            }

            return yOffset;
        }

        public float Height
        {
            get {
                return PACK_HEIGHT + MARGIN + this.assets.Count * (ASSET_HEIGHT + MARGIN);
            }
        }

        private const int PACK_HEIGHT = 25;
        private const int ASSET_HEIGHT = 20;
        private const int MARGIN = 1;
    }
}