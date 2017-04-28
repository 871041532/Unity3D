// -----------------------------------------------------------------
// File:    AssetManager.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    using UnityEngine.SceneManagement;
    using Version = GameBox.Framework.Version;

    class AssetManager : IAssetManager, IServiceGraph
    {
        public AssetManager()
        { }

        public string Id
        {
            get {
                return "com.giant.service.assetmanager";
            }
        }

        public void Run(IServiceRunner runner)
        {
            UpdateAsync(() =>
            {
                runner.Ready(_Terminate);
            });
        }

        public void Pulse(float delta)
        {
            this.timeInterval += delta;
            if (this.timeInterval < PACK_LIFE_CHECK_INTERVAL) {
                return;
            }

            var keys = new string[this.packs.Count];
            this.packs.Keys.CopyTo(keys, 0);
            for (var i = 0; i < keys.Length; ++i) {
                var pack = this.packs[keys[i]];
                if (1 == pack.RefCount && AssetPackState.LOADED == pack.State) {   // 1 means only AssetManager hold this asset pack
                    pack.LifeTime -= this.timeInterval;
                    if (pack.LifeTime <= 0) {
                        _ReleaseAssetPack(pack);
                        break;
                    }
                }
            }

            while (this.timeInterval >= PACK_LIFE_CHECK_INTERVAL) {
                this.timeInterval -= PACK_LIFE_CHECK_INTERVAL;
            }
        }

        public string AssetVersion
        {
            get {
                return this.info.Version;
            }
        }

        public IAsset Load(string path, AssetType type)
        {
            var asset = LoadAsset(path, type, true);
            return new AssetRef(asset);
        }

        public void LoadAsync(string path, AssetType type, Action<IAsset> handler)
        {
            LoadAssetAsync(path, type, true, (target, message) =>
            {
                var asset = target as Asset;
                if (null != handler) {
                    handler(new AssetRef(asset));
                }
            });
        }

        public IAsset LoadScene(string path, LoadSceneMode mode)
        {
            var asset = LoadAsset(path, AssetType.SCENE, true, mode);
            return new AssetRef(asset);
        }

        public void LoadSceneAsync(string path, LoadSceneMode mode, Action<IAsset> handler)
        {
            LoadAssetAsync(path, AssetType.SCENE, true, (target, message) =>
            {
                var asset = target as Asset;
                if (null != handler) {
                    handler(new AssetRef(asset));
                }
            }, mode);
        }

        [Obsolete]
        public IAssetLoader CreateLoader(string path, AssetType type)
        {
            IAssetLoader loader = null;
            switch (type) {
            case AssetType.TEXT:
                loader = new TextLoader(this, path);
                break;
            case AssetType.TEXTURE:
                loader = new TextureLoader(this, path);
                break;
            case AssetType.BYTES:
                loader = new BytesLoader(this, path);
                break;
            case AssetType.SPRITEATLAS:
                loader = new SpriteAtlasLoader(this, path);
                break;
            case AssetType.PREFAB:
                loader = new PrefabLoader(this, path);
                break;
            case AssetType.SCENE:
                loader = new SceneLoader(this, path);
                break;
            case AssetType.AUDIOCLIP:
                loader = new AudioClipLoader(this, path);
                break;
            case AssetType.ANIMATIONCONTROLLER:
                loader = new AnimationControllerLoader(this, path);
                break;
            default:
                Logger<IAssetManager>.E("Doesn't support this asset type: " + type);
                break;
            }

            return loader;
        }

        [Obsolete]
        public T CreateLoader<T>(string path) where T : IAssetLoader
        {
            if (typeof(T) == typeof(ITextLoader)) {
                return (T)(object)new TextLoader(this, path);
            } else if (typeof(T) == typeof(ITextureLoader)) {
                return (T)(object)new TextureLoader(this, path);
            } else if (typeof(T) == typeof(IBytesLoader)) {
                return (T)(object)new BytesLoader(this, path);
            } else if (typeof(T) == typeof(ISpriteAtlasLoader)) {
                return (T)(object)new SpriteAtlasLoader(this, path);
            } else if (typeof(T) == typeof(IPrefabLoader)) {
                return (T)(object)new PrefabLoader(this, path);
            } else if (typeof(T) == typeof(ISceneLoader)) {
                return (T)(object)new SceneLoader(this, path);
            } else if (typeof(T) == typeof(IAudioClipLoader)) {
                return (T)(object)new AudioClipLoader(this, path);
            } else if(typeof(T) == typeof(IAnimtionControllerLoader)){
                  return (T)(object)new AnimationControllerLoader(this, path);
            } else {
                Logger<IAssetManager>.E("Doesn't support this asset type: " + typeof(T).Name);
                return default(T);
            }
        }
        
        public void GC()
        {
            var keys = new string[this.packs.Count];
            this.packs.Keys.CopyTo(keys, 0);
            for (var i = 0; i < keys.Length; ++i) {
                var pack = this.packs[keys[i]];
                if (1 == pack.RefCount && AssetPackState.LOADED == pack.State) {
                    _ReleaseAssetPack(pack);
                }
            }
        }

        public void UpdateAsync(Action callback)
        {
            new FileReadTask(PathUtility.ComposeDataPath(Id))
            .Start()
            .Continue(task =>
            {
                if (null != task.Result) {
                    using (var stream = new MemoryStream(task.Result as byte[])) {
                        this.info = AssetManagerInfo.Deserialize(stream);
                        this.info._LoadType = AssetPackLoadType.PERSISTENTDATAPATH;
                        stream.Close();
                    }
                }

                return new WWWLoadTask(PathUtility.ComposeAppUrl(Id));
            })
            .Continue(task =>
            {
                if (null != task.Result) {
                    var w = task.Result as WWW;
                    if (null == w.error) {
                        using (var stream = new MemoryStream(w.bytes)) {
                            var info = AssetManagerInfo.Deserialize(stream);
                            info._LoadType = AssetPackLoadType.STREAMINGASSETS;
                            var innerVersion = new Version(info.Version);
                            var outerVersion = new Version(this.info.Version);
                            if (innerVersion.Compare(outerVersion) > 0) {
                                this.info = info;
                            }
                        }
                    }
                }

                return null;
            })
            .Continue(task =>
            {
                var reloadPacks = new List<AssetPack>();
                foreach (var pair in this.packs) {
                    var pack = pair.Value;
                    if (AssetPackLoadType.RESOURCES != pack.LoadType && AssetPackLoadType.REMOTE != pack.LoadType) { // DO NOT reload asset in Resources
                        pack._Reset(false); // Reset first
                        reloadPacks.Add(pack);
                    }
                }

                if (reloadPacks.Count > 0) {
                    var count = reloadPacks.Count;
                    for (var i = 0; i < reloadPacks.Count; ++i) {
                        var pack = reloadPacks[i];
                        pack.LoadType = this.info._LoadType; // Sync load type after _Reset
                        pack.LoadAsync((assetPack, _) =>
                        {
                            if (0 == (--count) && null != callback) {
                                callback();
                            }
                        });
                    }
                } else if (null != callback) {
                    callback();
                }

                return null;
            });
        }

        public Asset LoadAsset(string path, AssetType type, bool checkDependency, params object[] more)
        {
            var pack = _GetAssetPack(path, checkDependency, type);
            pack.Load();

            if (AssetPackState.LOADED == pack.State) {
                return pack.GetAsset(path, type, more);
            } else {
                _ReleaseAssetPack(pack);
                return null;
            }
        }

        public void LoadAssetAsync(string path, AssetType type, bool checkDependency, Action<object, Message> callback, params object[] more)
        {
            var pack = _GetAssetPack(path, checkDependency, type);
            pack.LoadAsync((target, message) =>
            {
                if (AssetPackState.LOADED == pack.State) {
                    pack.GetAssetAsync(path, type, callback, more);
                } else {
                    _ReleaseAssetPack(pack);
                    callback(null, Message.Any);
                }
            });
        }

        public AssetPack LoadAssetPack(string path, bool checkDependency)
        {
            var pack = _GetAssetPack(path, checkDependency, AssetType.UNKNOWN);
            pack.Load();

            if (AssetPackState.LOADED == pack.State) {
                pack.Retain();
                return pack;
            } else {
                _ReleaseAssetPack(pack);
                return null;
            }
        }

        public void LoadAssetPackAsync(string path, bool checkDependency, Action<object, Message> callback)
        {
            var pack = _GetAssetPack(path, checkDependency, AssetType.UNKNOWN);
            pack.LoadAsync((target, message) =>
            {
                if (AssetPackState.LOADED == pack.State) {
                    pack.Retain();
                    callback(pack, Message.Any);
                } else {
                    _ReleaseAssetPack(pack);
                    callback(null, Message.Any);
                }
            });
        }

        public string[] GetDependence(string path)
        {
            if (this.info.Packs.ContainsKey(path)) {
                return this.info.Packs[path].Dependencies;
            }

            return null;
        }

        private void _Terminate()
        {
            foreach (var pack in this.packs.Values) {
                pack.Release();
            }
            this.packs = null;
        }

        private AssetPack _GetAssetPack(string path, bool checkDependency, AssetType assetType)
        {
            AssetPack pack = null;
            lock (this.packs) {
                var loadType = this.info._LoadType;
                var packInfo = _FindAssetPackInfo(path);
                if (null == packInfo) {
                    var assetInfo = _FindAssetInfo(path);
                    if (null != assetInfo) {
                        path = assetInfo.PackPath;  // Override path with pack path
                        packInfo = _FindAssetPackInfo(path);
                        if (null == packInfo) {
                            Logger<IAssetManager>.E("!IMPOSSIBLE!");
                        }
                    } else {
                        packInfo = new AssetPackInfo {
                            Type = AssetPackType.FLAT,
                        };
                        if (File.Exists(PathUtility.ComposeDataPath(path))) {   // Check if it exists but not managed by AssetManager
                            loadType = AssetPackLoadType.PERSISTENTDATAPATH;
                        } else if (AssetType.SCENE == assetType) {              // Unity scene type
                            loadType = AssetPackLoadType.INTERNALSCENE;
                        } else {
                            loadType = AssetPackLoadType.RESOURCES;
                            if (AssetType.SPRITEATLAS == assetType) {           // Resources sprite atlas
                                packInfo.Type = AssetPackType.BUNDLE;
                            }
                        }
                        this.info.Packs.Add(path, packInfo);
                    }
                }

                if (!this.packs.TryGetValue(path, out pack)) {
                    pack = new AssetPack(this, path, packInfo.Type, (path.StartsWith("http") ? AssetPackLoadType.REMOTE : loadType), checkDependency);
                    _RetainAssetPack(pack);
                }
            }

            return pack;
        }

        private AssetPackInfo _FindAssetPackInfo(string path)
        {
            if (this.info.Packs.ContainsKey(path)) {
                return this.info.Packs[path];
            }

            path = path.ToLower();
            if (this.info.Packs.ContainsKey(path)) {
                return this.info.Packs[path];
            }

            return null;
        }

        private AssetInfo _FindAssetInfo(string path)
        {
            if (this.info.Assets.ContainsKey(path)) {
                return this.info.Assets[path];
            }

            path = path.ToLower();
            if (this.info.Assets.ContainsKey(path)) {
                return this.info.Assets[path];
            }

            return null;
        }

        private void _RetainAssetPack(AssetPack pack)
        {
            pack.Retain();
            lock (this.packs) {
                this.packs.Add(pack.Path, pack);
            }
        }

        private void _ReleaseAssetPack(AssetPack pack)
        {
            lock (this.packs) {
                this.packs.Remove(pack.Path);
            }

            pack.Release();
        }

        private AssetManagerInfo info = new AssetManagerInfo();
        private Dictionary<string, AssetPack> packs = new Dictionary<string, AssetPack>();
        private float timeInterval = 0f;

        private const float PACK_LIFE_CHECK_INTERVAL = 1f;  // 1 second

        // ---------------------------------------------------------
        // Graph
        public void Draw()
        {
            float yOffset = 1;
            foreach (var pair in this.packs) {
                var pack = pair.Value;
                yOffset = pack.DrawGraph(yOffset);
            }
        }

        public float Width
        {
            get {
                return GraphStyle.ServiceWidth;
            }
        }

        public float Height
        {
            get {
                var height = 0f;
                foreach (var pair in this.packs) {
                    height += pair.Value.Height;
                }

                return height;
            }
        }
    }
}