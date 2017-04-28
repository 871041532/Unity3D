// -----------------------------------------------------------------
// File:    AssetsLoadFromBundleTask.cs
// Author:  mouguangyi
// Date:    2016.10.26
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    class AssetsLoadFromBundleTask : AsyncOperationTask<AssetBundleRequest>
    {
        public AssetsLoadFromBundleTask(AssetBundle bundle) : base(() => { return bundle.LoadAllAssetsAsync(); }, false)
        { }
    }

    class AssetsLoadFromBundleTask<T> : AsyncOperationTask<AssetBundleRequest> where T : Object
    {
        public AssetsLoadFromBundleTask(AssetBundle bundle) : base(() => { return bundle.LoadAllAssetsAsync<T>(); }, false)
        { }
    }
}