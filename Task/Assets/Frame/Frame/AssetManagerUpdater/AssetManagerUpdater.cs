// -----------------------------------------------------------------
// File:    AssetManagerUpdater.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using GameBox.Service.AssetUpdater;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameBox.Service.AssetManagerUpdater
{
    sealed class AssetManagerUpdater : IAssetManagerUpdater, IAssetConfigureParser
    {
        public string Id
        {
            get {
                return "com.giant.service.assetmanagerupdater";
            }
        }

        public void Run(IServiceRunner runner)
        {
            this.appServer = runner.GetArgs<string>("AppServer");
            this.assetServer = runner.GetArgs<string>("AssetServer");
            this.valid = (runner.GetArgs<bool>("Valid") && !string.IsNullOrEmpty(this.assetServer));

            new ServicesTask(new string[] {
                "com.giant.service.assetmanager",
                "com.giant.service.assetlistupdater",
            }).Start().Continue(task =>
            {
                var services = task.Result as IService[];
                this.manager = services[0] as IAssetManager;
                this.updater = services[1] as IAssetListUpdater;

                runner.Ready(_Terminate);

                return null;
            });
        }

        public void Pulse(float delta)
        { }

        // IAssetManagerUpdater
        public void Check(Action<AssetUpdateType, IAssetDownloader> callback)
        {
            if (!this.valid) {
                callback(AssetUpdateType.UPDATED, null);
                return;
            }

            var platform = "";
            switch (Application.platform) {
            case RuntimePlatform.Android:
                platform = "android/";
                break;
            case RuntimePlatform.IPhonePlayer:
                platform = "ios/";
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                platform = "windows/";
                break;
            }

            this.updater.Check(this.assetServer + platform, this.manager.Id, this, callback);
        }

        public string AppServer
        {
            get {
                return this.appServer;
            }
        }

        // IAssetConfigureParser
        public void ParseAsync(byte[] data, Action<IAssetConfigure> handler)
        {
            using (var stream = new MemoryStream(data, 0, data.Length)) {
                var managerInfo = AssetManagerInfo.Deserialize(stream);
                new CompletedTask().Start().Continue(task =>
                {
                    if (null != handler) {
                        handler(new ManagerInfoBridge(managerInfo));
                    }
                    return null;
                });
            }
        }

        private void _Terminate()
        { }

        private class ManagerInfoBridge : IAssetConfigure
        {
            public ManagerInfoBridge(AssetManagerInfo managerInfo)
            {
                this.version = managerInfo.Version;

                var packs = managerInfo.Packs;
                var assets = new List<IAssetDescription>();
                foreach (var item in packs) {
                    assets.Add(new PackInfoBridge(item.Key, item.Value));
                }
                this.assets = assets.ToArray();
            }

            public string Version
            {
                get {
                    return this.version;
                }
            }

            public IAssetDescription[] Assets
            {
                get {
                    return this.assets;
                }
            }

            private string version = null;
            private IAssetDescription[] assets = null;
        }

        private class PackInfoBridge : IAssetDescription
        {
            public PackInfoBridge(string path, AssetPackInfo packInfo)
            {
                this.path = path;
                this.packInfo = packInfo;
            }

            public bool Equals(byte[] data)
            {
                return (CryptoUtility.ComputeMD5Code(data) == this.packInfo.CheckCode);
            }

            public string Path
            {
                get {
                    return this.path;
                }
            }

            public long Size
            {
                get {
                    return this.packInfo.Size;
                }
            }

            private string path = null;
            private AssetPackInfo packInfo = null;
        }

        private string appServer = null;
        private string assetServer = null;
        private bool valid = true;
        private IAssetManager manager = null;
        private IAssetListUpdater updater = null;
    }
}