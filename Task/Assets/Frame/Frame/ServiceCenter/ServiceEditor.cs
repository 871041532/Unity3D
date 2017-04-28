// -----------------------------------------------------------------
// File:    ServiceEditor.cs
// Author:  mouguangyi
// Date:    2016.11.17
// Description:
//      
// -----------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// @details Service在Unity编辑模式下的运行环境。可以在Unity非运行模式下启动需要的某一个服务。
    /// 
    /// @section example 例子
    /// 运行资产管理服务。
    /// 
    /// @code{.cs}
    /// var editor = new ServiceEditor();
    /// editor.Start<AssetManagerInstaller>();
    /// // 使用服务
    /// ...
    /// 
    /// // 销毁
    /// editor.OnDestroy();
    /// @endcode
    /// </summary>
    public sealed class ServiceEditor : IServiceHost
    {
        /// <summary>
        /// 启动指定服务。
        /// </summary>
        /// <typeparam name="T">服务对应的安装器。</typeparam>
        public void Start<T>() where T : ServiceBaseInstaller
        {
            this.asyncTerminateFlag = false;
            this.asyncWait = new AutoResetEvent(false);
            this.asyncThread = new Thread(new ThreadStart(_RunAsyncThread));
            this.asyncThread.Start();

            this.center = new ServiceCenter();
            this.center._Enter(this);

            var go = new GameObject();
            go.AddComponent<T>();
            {
                var installers = go.GetComponents<ServiceBaseInstaller>();
                for (var i = 0; i < installers.Length; ++i) {
                    var installer = installers[i];
                    if (null != installer) {
                        installer._Install(this.center);
                    }
                }
            }
            GameObject.DestroyImmediate(go);

            var serviceCount = this.center._RunnerCount;
            for (var i = 0; i < serviceCount; ++i) {
                var runner = this.center._GetRunnerByIndex(i);
                runner.Startup();
            }
        }

        /// <summary>
        /// 帧刷新入口。有些Service需要不停的刷新，因此需要不停的调用该方法来触发Service的刷新。使用者可以绑定EditorApplication.update句柄来触发该方法。
        /// </summary>
        /// <param name="delta">帧间隔。以秒为单位。</param>
        public void Update(float delta)
        {
            this.center._Execute(delta);

            if (this.mainTasks.Count > 0) {
                _ExecuteAsyncTasks(this.mainTasks);
            }
        }

        /// <summary>
        /// 销毁方法。
        /// @note 使用结束后需要调用该方法来清理资源。
        /// </summary>
        public void OnDestroy()
        {
            this.center._Exit();
            this.center = null;

            if (null != this.asyncThread) {
                this.asyncTerminateFlag = true;
                this.asyncWait.Set();
                this.asyncThread = null;
            }
        }

        public void RunAsyncTask(AsyncTask task)
        {
            if (task.CanRunOnThread) {
                lock (this.threadTasks) {
                    this.threadTasks.AddLast(task.GetExecuter());
                }
                this.asyncWait.Set();
            } else {
                lock (this.mainTasks) {
                    this.mainTasks.AddLast(task.GetExecuter());
                }
            }
        }

        private void _RunAsyncThread()
        {
            while (!this.asyncTerminateFlag) {
                if (0 == this.threadTasks.Count) {
                    this.asyncWait.WaitOne();
                }

                _ExecuteAsyncTasks(this.threadTasks);
            }

            this.threadTasks.Clear();
        }

        private void _ExecuteAsyncTasks(LinkedList<IEnumerator> tasks)
        {
            var executor = tasks.First;
            while (null != executor) {
                var current = executor;
                executor = executor.Next;

                if (!current.Value.MoveNext()) {
                    lock (tasks) {
                        tasks.Remove(current);
                    }
                }
            }
        }

        private ServiceCenter center = null;
        private LinkedList<IEnumerator> mainTasks = new LinkedList<IEnumerator>();
        private volatile bool asyncTerminateFlag = false;
        private AutoResetEvent asyncWait = null;
        private Thread asyncThread = null;
        private LinkedList<IEnumerator> threadTasks = new LinkedList<IEnumerator>();
    }
}