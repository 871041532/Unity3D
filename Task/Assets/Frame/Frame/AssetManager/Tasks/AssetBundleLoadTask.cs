// -----------------------------------------------------------------
// File:    AssetBundleLoadTask.cs
// Author:  mouguangyi
// Date:    2016.06.28
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    // AssetBundle“Ï≤Ωº”‘ÿTask°£
    class AssetBundleLoadTask : AsyncOperationTask<AssetBundleCreateRequest>
    {
        public AssetBundleLoadTask(string path) : base(() => { return AssetBundle.LoadFromFileAsync(path); }, false)
        { }
    }
}