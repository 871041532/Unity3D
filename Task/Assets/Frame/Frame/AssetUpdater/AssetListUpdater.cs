// -----------------------------------------------------------------
// File:    AssetListUpdater.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameBox.Service.AssetUpdater
{
    sealed class AssetListUpdater : IAssetListUpdater, IAssetDownloader, IServiceGraph
    {
        public string Id
        {
            get {
                return "com.giant.service.assetlistupdater";
            }
        }

        public void Run(IServiceRunner runner)
        {
            runner.Ready(_Terminate);
        }

        public void Pulse(float delta)
        {
            if (this.downloading) {
                _UpdateDownloadedSize();
            }
        }

        private void _Terminate()
        { }

        // IAssetListUpdater
        public void Check(string updateServerPath, string configureFile, IAssetConfigureParser parser, Action<AssetUpdateType, IAssetDownloader> callback)
        {
            this.updateServerPath = updateServerPath;
            this.configureFile = configureFile;
            this.callback = callback;

            if (!Directory.Exists(PathUtility.DataFolder)) {
                Directory.CreateDirectory(PathUtility.DataFolder);
            }

            byte loadType = PERSISTENTDATAPATH;

            new HttpDownloadTask(this.updateServerPath + configureFile + "?")    // Add time out (5s) for request
            .Start()
            .Continue(task =>
            {
                if (null != task.Result) {  // Check null first since IL2CPP will throw exception if converting null to any type
                    this.configureData = task.Result as byte[];
                    return new AssetConfigureParseTask(parser, this.configureData);
                }

                return null;
            })
            .Continue(task =>
            {
                remoteConfigure = _CastToAssetConfigure(task.Result);

                var filePath = PathUtility.ComposeDataPath(configureFile);
                return new FileReadTask(filePath);
            })
            .Continue(task =>
            {
                return (null != task.Result ? new AssetConfigureParseTask(parser, task.Result as byte[]) : null);
            })
            .Continue(task =>
            {
                this.localConfigure = _CastToAssetConfigure(task.Result);

                var fileUrl = PathUtility.ComposeAppUrl(configureFile);
                return new WWWLoadTask(fileUrl, 5.0f);
            })
            .Continue(task =>
            {
                if (null != task.Result) {  // Check null first since IL2CPP will throw exception if converting null to any type
                    var w = task.Result as WWW;
                    if (null == w.error) {
                        return new AssetConfigureParseTask(parser, w.bytes);
                    }
                }

                return null;
            })
            .Continue(task =>
            {
                var assetConfigure = _CastToAssetConfigure(task.Result);
                if (null != assetConfigure) {
                    if (null == this.localConfigure) {
                        this.localConfigure = assetConfigure;
                        loadType = STREAMINGASSETS;
                    } else {
                        var innerVersion = new GameBox.Framework.Version(assetConfigure.Version);
                        var outerVersion = new GameBox.Framework.Version(this.localConfigure.Version);
                        if (innerVersion.Compare(outerVersion) > 0) {
                            this.localConfigure = assetConfigure;
                            loadType = STREAMINGASSETS;
                        }
                    }
                }

                if (null == this.remoteConfigure) {
                    _NotifyCallback(this.updateType = AssetUpdateType.INVALID, null);
                } else if (null == this.localConfigure) {
                    _NotifyCallback(this.updateType = AssetUpdateType.HOTUPDATE, _SetupHotUpdateDownloaderWithoutCheck(this.updateServerPath, remoteConfigure));
                } else {
                    var localVersion = new GameBox.Framework.Version(this.localConfigure.Version);
                    var remoteVersion = new GameBox.Framework.Version(this.remoteConfigure.Version);
                    if (remoteVersion.Major != localVersion.Major) {
                        // Major version upgrade indicates full app upgrade
                        _NotifyCallback(this.updateType = AssetUpdateType.FULLUPDATE, null);
                    } else if (0 != remoteVersion.Compare(localVersion)) {
                        // Major version is same but minor version is upgrade indicates hot assets upgrade
                        _SetupHotUpdateDownloaderAsync(this.updateServerPath, loadType, this.remoteConfigure, downloader =>
                        {
                            _NotifyCallback(this.updateType = (null != downloader ? AssetUpdateType.HOTUPDATE : AssetUpdateType.UPDATED), downloader);
                        });
                    } else {
                        _NotifyCallback(this.updateType = AssetUpdateType.UPDATED, null);
                    }
                }

                return null;
            });
        }

        // IAssetDownloader
        public string AssetVersion
        {
            get {
                return this.assetVersion;
            }
        }

        public long TotalSize
        {
            get {
                return this.totalSize;
            }
        }

        public long DownloadedSize
        {
            get {
                return this.downloadedSize;
            }
        }

        public void Start(Action handler, int parallelCount = -1)
        {
            this.downloading = true;
            this.handler = handler;
            new CompletedTask().Start().Continue(task =>
            {
                this.downloadIndex = 0;
                this.downloadedCount = 0;
                if (-1 == parallelCount) {
                    parallelCount = this.assets.Count;
                }
                while (this.downloadIndex < parallelCount && this.downloadIndex < this.assets.Count) {
                    this.assets[this.downloadIndex++].Download(_OnDownloadedAsset);
                }

                return null;
            });
        }

        private IAssetConfigure _CastToAssetConfigure(object o)
        {
            return (null != o ? o as IAssetConfigure : null);
        }

        private void _NotifyCallback(AssetUpdateType type, IAssetDownloader downloader)
        {
            if (null != this.callback) {
                this.callback(type, downloader);
            }
        }

        private IAssetDownloader _SetupHotUpdateDownloaderWithoutCheck(string updateServerPath, IAssetConfigure assetConfigure)
        {
            this.assetVersion = assetConfigure.Version;
            this.totalSize = 0;
            this.assets.Clear();

            var assetDescs = assetConfigure.Assets;
            for (var i = 0; i < assetDescs.Length; ++i) {
                var description = assetDescs[i];

                this.assets.Add(new Asset(description) {
                    ServerPath = updateServerPath,
                    File = description.Path,
                    Size = description.Size,
                });
                this.totalSize += description.Size;
            }

            return this;
        }

        private void _SetupHotUpdateDownloaderAsync(string updateServerPath, byte loadType, IAssetConfigure assetConfigure, Action<IAssetDownloader> callback)
        {
            this.assetVersion = assetConfigure.Version;
            this.totalSize = 0;
            this.assets.Clear();

            var assetDescs = assetConfigure.Assets;
            var checkLeftCount = assetDescs.Length;
            for (var i = 0; i < assetDescs.Length; ++i) {
                var description = assetDescs[i];
                _CollectUpgradeAssetAsync(loadType, updateServerPath, description, asset =>
                {
                    if (null != asset) {
                        this.assets.Add(asset);
                        this.totalSize += asset.Size;
                    }

                    if (0 == (--checkLeftCount)) {
                        if (this.assets.Count > 0) {
                            callback(this);
                        } else {
                            _SaveConfigureDataAsync(() =>
                            {
                                callback(null);
                            });
                        }
                    }
                });
            }
        }

        private void _CollectUpgradeAssetAsync(byte loadType, string updateServerPath, IAssetDescription description, Action<Asset> callback)
        {
            if (STREAMINGASSETS == loadType || !File.Exists(PathUtility.ComposeDataPath(description.Path))) {
                _CheckStreamingAssetAsync(description.Path, description, result =>
                {
                    callback(result ? new Asset(description) {
                        ServerPath = updateServerPath,
                        File = description.Path,
                        Size = description.Size,
                    } : null);
                });
            } else {
                _CheckPerisitentDataAssetAsync(description.Path, description, result =>
                {
                    callback(result ? new Asset(description) {
                        ServerPath = updateServerPath,
                        File = description.Path,
                        Size = description.Size,
                    } : null);
                });
            }
        }

        private void _CheckPerisitentDataAssetAsync(string path, IAssetDescription description, Action<bool> callback)
        {
            var fullPath = PathUtility.ComposeDataPath(path);
            new FileReadTask(fullPath).Start().Continue(task =>
            {
                var bytes = task.Result as byte[];
                callback(!description.Equals(bytes));

                return null;
            });
        }

        private void _CheckStreamingAssetAsync(string path, IAssetDescription description, Action<bool> callback)
        {
            var fullPath = PathUtility.ComposeAppUrl(path);
            new WWWLoadTask(fullPath, 1f).Start().Continue(task =>
            {
                if (null != task.Result) {
                    var w = task.Result as WWW;
                    callback(null != w.error || !description.Equals(w.bytes));
                } else {
                    callback(true);
                }
                return null;
            });
        }

        private void _OnDownloadedAsset(Asset asset)
        {
            if (this.downloadIndex < this.assets.Count) {
                this.assets[this.downloadIndex++].Download(_OnDownloadedAsset);
            }
            if ((++this.downloadedCount) == this.assets.Count) {
                _SaveConfigureDataAsync(() =>
                {
                    _OnDownloaded();
                });
            }
        }

        private void _SaveConfigureDataAsync(Action callback)
        {
            var filePath = Path.Combine(Application.persistentDataPath, this.configureFile);
            new FileWriteTask(filePath, this.configureData).Start().Continue(task =>
            {
                if (null != callback) {
                    callback();
                }

                return null;
            });
        }

        private void _OnDownloaded()
        {
            this.downloading = false;
            if (null != this.handler) {
                this.handler();
                this.handler = null;
            }
        }

        private void _UpdateDownloadedSize()
        {
            this.downloadedSize = 0;
            for (var i = 0; i < this.assets.Count; ++i) {
                this.downloadedSize += this.assets[i].DownloadedSize;
            }
        }

        // IAssetListUpdater
        private string updateServerPath = null;
        private string configureFile = null;
        private byte[] configureData = null;
        private IAssetConfigure remoteConfigure = null;
        private IAssetConfigure localConfigure = null;
        private AssetUpdateType updateType = AssetUpdateType.INVALID;
        private Action<AssetUpdateType, IAssetDownloader> callback = null;

        // IAssetDownloader
        private string assetVersion = null;
        private List<Asset> assets = new List<Asset>();
        private long totalSize = 0;
        private long downloadedSize = 0;
        private Action handler = null;
        private int downloadIndex = 0;
        private int downloadedCount = 0;
        private bool downloading = false;

        private const byte STREAMINGASSETS = 0;
        private const byte PERSISTENTDATAPATH = 1;

        private class AssetConfigureParseTask : AsyncTask
        {
            public AssetConfigureParseTask(IAssetConfigureParser parser, byte[] data) : base(true)
            {
                parser.ParseAsync(data, assets =>
                {
                    this.Result = assets;
                    this.completed = true;
                });
            }

            protected override bool IsDone()
            {
                return this.completed;
            }

            private bool completed = false;
        }

        // - IServiceGraph
        public void Draw()
        {
            var yOffset = 0f;
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), GraphStyle.GreenTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), "Server:\t" + this.updateServerPath, GraphStyle.MiniLabel);

            yOffset += INFO_HEIGHT;
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), GraphStyle.GreenTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), "Remote version:\t" + (null != this.remoteConfigure ? this.remoteConfigure.Version : "Invalid"), GraphStyle.MiniLabel);

            yOffset += INFO_HEIGHT;
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), GraphStyle.GreenTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), "Local version:\t" + (null != this.localConfigure ? this.localConfigure.Version : "Invalid"), GraphStyle.MiniLabel);

            yOffset += INFO_HEIGHT;
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), GraphStyle.GreenTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, INFO_HEIGHT), "Update status:\t" + this.updateType.ToString(), GraphStyle.MiniLabel);
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
                return INFO_HEIGHT * 4;
            }
        }

        private const int INFO_HEIGHT = 26;
    }
}