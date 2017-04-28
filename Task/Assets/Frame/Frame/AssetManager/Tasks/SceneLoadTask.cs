// -----------------------------------------------------------------
// File:    SceneLoadTask.cs
// Author:  mouguangyi
// Date:    2016.06.29
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameBox.Service.AssetManager
{
    // 场景异步加载任务。
    class SceneLoadTask : AsyncOperationTask<AsyncOperation>
    {
        public SceneLoadTask(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) : base(() => { return SceneManager.LoadSceneAsync(sceneName, mode); }, false)
        { }
    }
}