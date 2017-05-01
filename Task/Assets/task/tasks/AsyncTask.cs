
using System;
using System.Collections;

namespace GameFramework
{
    /// <summary>
    /// @details 异步任务基类。
    /// </summary>
    public class AsyncTask 
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="canRunOnThread">是否能运行在子线程上。</param>
        public AsyncTask(bool canRunOnThread)
        {
            this.canRunOnThread = canRunOnThread;
            this.executer = new Executer(this);
        }

        /// <summary>
        /// 开始任务。
        /// </summary>
        /// <returns>返回开始的任务。</returns>
        public AsyncTask Start()
        {
            this.center = TaskCenter.Center();
            var task = _SearchValidTaskFrom(this);
            if (null != task)
            {
                _StartTask(task);
            }

            return this;
        }

        /// <summary>
        /// Task结束后继续处理。
        /// </summary>
        /// <param name="handler">继续处理的函数句柄。该句柄是以上一个任务作为传入参数，而需要返回下一个需要执行的任务。若返回非AsyncTask类型，则认为是任务结束，并以该返回值作为结束任务的Result。</param>
        /// <returns>返回一个任务。</returns>
        public AsyncTask Continue(Func<AsyncTask, object> handler)
        {
            this.handler = handler;
            this.child = new InternalTask();
            this.child.parent = this;
            return this.child;
        }

        /// <summary>
        /// Task结果。
        /// </summary>
        public object Result
        {
            get; protected set;
        }

        /// <summary>
        /// Task是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected virtual bool IsDone()
        {
            return true;
        }

        internal Executer GetExecuter()
        {
            return this.executer;
        }

        internal bool CanRunOnThread
        {
            get
            {
                return this.canRunOnThread;
            }
        }

        private AsyncTask _NextTask(object result)
        {
            if (result is AsyncTask)
            {
                var task = result as AsyncTask;
                task.child = this.child.child;
                task.handler = this.child.handler;

                return _SearchValidTaskFrom(task);
            }
            else
            {
                this.child.Result = result;

                return this.child;
            }
        }

        private AsyncTask _SearchValidTaskFrom(AsyncTask task)
        {
            while (true)
            {
                if (task.isExecuting)
                {
                    return null;
                }
                else if (null == task.parent)
                {
                    return task;
                }

                task = task.parent;
            }
        }

        private void _StartTask(AsyncTask task)
        {
            task.center = this.center;
            task.isExecuting = true;
            this.center._RunAsyncTask(task);
        }

        private void _CompleteTask()
        {
            if (null != this.handler)
            {
                this.center._RunOnGameThread(() =>
                {
                    var task = this._NextTask(this.handler(this));
                    if (null != task)
                    {
                        this._StartTask(task);
                    }
                });
            }
        }

        internal TaskCenter center = null;
        private bool canRunOnThread = true;
        private bool isExecuting = false;
        private Func<AsyncTask, object> handler = null;
        private AsyncTask parent = null;
        private AsyncTask child = null;
        private Executer executer = null;

        internal class Executer : IEnumerator
        {
            public Executer(AsyncTask task)
            {
                this.task = task;
            }

            public object Current
            {
                get
                {
                    return null;
                }
            }

            public bool MoveNext()//此处决定了在迭代过程中是否移向下一步（因而能异步）
            {
                if (!this.task.IsDone())//自身逻辑未做完
                {
                    return true;
                }
                //自身逻辑已经做完，回调handler加入center待执行协程列表
                this.task._CompleteTask();
                return false;
            }

            public void Reset()
            { }

            private AsyncTask task = null;
        }

        internal class InternalTask : AsyncTask
        {
            public InternalTask() : base(true)
            { }
        }
    }
}
