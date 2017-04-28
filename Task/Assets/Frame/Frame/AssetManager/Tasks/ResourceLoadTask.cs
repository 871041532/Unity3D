// -----------------------------------------------------------------
// File:    ResourceLoadTask.cs
// Author:  mouguangyi
// Date:    2016.06.28
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.AssetManager
{
    // Resource异步加载Task。
    class ResourceLoadTask : AsyncOperationTask<ResourceRequest>
    {
        public ResourceLoadTask(string path) : base(() => { return Resources.LoadAsync(path); }, false)
        { }
    }

    // Resource范化异步加载Task。
    class ResourceLoadTask<T> : AsyncOperationTask<ResourceRequest> where T : Object
    {
        public ResourceLoadTask(string path) : base(() => { return Resources.LoadAsync<T>(path); }, false)
        { }
    }

    // Resource泛化异步加载所有Task。
    class ResourceLoadAllTask<T> : AsyncCallWithResultTask<T[]> where T : Object
    {
        public ResourceLoadAllTask(string path) : base(() => { return Resources.LoadAll<T>(path); }, false)
        { }
    }
}