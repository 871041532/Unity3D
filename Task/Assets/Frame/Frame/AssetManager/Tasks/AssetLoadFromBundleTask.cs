// -----------------------------------------------------------------
// File:    AssetLoadFromBundleTask.cs
// Author:  mouguangyi
// Date:    2016.07.20
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    // ��AssetBundle���첽װ��Asset
    class AssetLoadFromBundleTask : AsyncOperationTask<AssetBundleRequest>
    {
        public AssetLoadFromBundleTask(AssetBundle bundle, string path) : base(() => { return bundle.LoadAssetAsync(path); }, false)
        { }
    }
}