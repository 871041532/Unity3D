// -----------------------------------------------------------------
// File:    Asset.cs
// Author:  mouguangyi
// Date:    2016.06.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.IO;
using UnityEngine;

namespace GameBox.Service.AssetUpdater
{
    class Asset
    {
        public Asset(IAssetDescription description)
        {
            this.description = description;
        }

        public string ServerPath { get; set; }

        public string File { get; set; }

        public long Size { get; set; }

        public long DownloadedSize
        {
            get {
                return (null != this.task ? this.task.DownloadedSize : 0);
            }
        }

        public void Download(Action<Asset> callback)
        {
            this.task = new HttpDownloadTask(this.ServerPath + this.File);
            this.task.Start()
            .Continue(task =>
            {
                if (null != task.Result) {
                    var filePath = Path.Combine(Application.persistentDataPath, this.File);
                    var data = task.Result as byte[];
                    return new FileWriteTask(filePath, data);
                } else {
                    return null;
                }
            })
            .Continue(task =>
            {
                // TODO: Verify the downloaded file is correct or not by description.Equals function

                Logger<IAssetListUpdater>.L(this.File + " has been downloaded and total size is " + this.DownloadedSize + " bytes.");
                if (null != callback) {
                    callback(this);
                }

                return null;
            });
        }

        private IAssetDescription description = null;
        private HttpDownloadTask task = null;
    }
}