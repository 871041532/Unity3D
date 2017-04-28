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
    // Resource�첽����Task��
    class ResourceLoadTask : AsyncOperationTask<ResourceRequest>
    {
        public ResourceLoadTask(string path) : base(() => { return Resources.LoadAsync(path); }, false)
        { }
    }

    // Resource�����첽����Task��
    class ResourceLoadTask<T> : AsyncOperationTask<ResourceRequest> where T : Object
    {
        public ResourceLoadTask(string path) : base(() => { return Resources.LoadAsync<T>(path); }, false)
        { }
    }

    // Resource�����첽��������Task��
    class ResourceLoadAllTask<T> : AsyncCallWithResultTask<T[]> where T : Object
    {
        public ResourceLoadAllTask(string path) : base(() => { return Resources.LoadAll<T>(path); }, false)
        { }
    }
}