
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 绑定于uintyGameObject上，开辟task逻辑专用线程，并负责LateUpdate执行任务中心放法
    /// 线程主要用来跑while循环用于迭代时moveNext询问
    /// </summary>
    public sealed class TaskPlayer : MonoBehaviour
    {
        public void Awake()
        {
            this.asyncTerminateFlag = false;
            this.asyncWait = new AutoResetEvent(false);
            this.asyncThread = new Thread(new ThreadStart(_RunAsyncThread));
            this.asyncThread.Start();

            this.center = new TaskCenter();
            this.center._Enter(this);

            DontDestroyOnLoad(this.gameObject);
        }

        public void LateUpdate()
        {
            this.center._Execute(Time.deltaTime);
        }

        public void OnDestroy()
        {
            this.center._Exit();
            this.center = null;

            if (null != this.asyncThread)
            {
                this.asyncTerminateFlag = true;
                this.asyncWait.Set();
                this.asyncThread = null;
            }
        }

        public void RunAsyncTask(AsyncTask task)
        {
            if (task.CanRunOnThread)
            {
                lock (this.asyncTasks)
                {
                    this.asyncTasks.AddLast(task.GetExecuter());//立即执行
                }
                this.asyncWait.Set();
            }
            else
            {
                StartCoroutine(task.GetExecuter());//利用协程等待IEnumerator
            }
        }

        private void _RunAsyncThread()
        {
            while (!this.asyncTerminateFlag)
            {
                if (0 == this.asyncTasks.Count)
                {
                    this.asyncWait.WaitOne();
                }

                var executor = this.asyncTasks.First;
                while (null != executor)
                {
                    var current = executor;
                    executor = executor.Next;

                    if (!current.Value.MoveNext())
                    {
                        lock (this.asyncTasks)
                        {
                            this.asyncTasks.Remove(current);
                        }
                    }
                }
            }

            this.asyncTasks.Clear();
        }

        private TaskCenter center = null;
        private volatile bool asyncTerminateFlag = false;
        private AutoResetEvent asyncWait = null;
        private Thread asyncThread = null;
        private LinkedList<IEnumerator> asyncTasks = new LinkedList<IEnumerator>();
    }
}