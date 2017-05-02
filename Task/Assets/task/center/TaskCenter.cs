using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 内部list存储将要执行的回调，并处理迭代逻辑
    /// </summary>
    public class TaskCenter
    {
        internal void _Enter(TaskPlayer serverPlayer)
        {
            this.servicePlayer = serverPlayer;

            if (null != TaskCenter.instance)
            {
                throw new Exception("Duplicated TaskCenter component！");
            }
            TaskCenter.instance = this;
        }
        internal void _Execute(float delta)
        {
            if (this.isActionQueueEmpty)
            {
                return;
            }
            this.localActionQueue.Clear();
            lock (this.actionQueue)
            {
                this.localActionQueue.AddRange(this.actionQueue);
                this.actionQueue.Clear();
                this.isActionQueueEmpty = true;
            }
            for (var i = 0; i < this.localActionQueue.Count; ++i)
            {
                this.localActionQueue[i].Invoke();//跨线程调用（使用主线程调用委托）
            }
        }

        internal void _Exit()
        {
            this.servicePlayer = null;
            TaskCenter.instance = null;
        }

        internal void _RunOnGameThread(Action action)
        {
            if (null == action)
            {
                throw new ArgumentNullException("Action");
            }

            lock (this.actionQueue)//线程等待
            {
                this.actionQueue.Add(action);
                this.isActionQueueEmpty = false;
            }
        }

        internal void _RunAsyncTask(AsyncTask task)
        {
            _RunOnGameThread(() =>
            {
                this.servicePlayer.RunAsyncTask(task);
            });
        }

        private TaskPlayer servicePlayer = null;
        private bool ready = true;
        private bool isActionQueueEmpty = true;
        private List<Action> actionQueue = new List<Action>();
        private List<Action> localActionQueue = new List<Action>();
        private int graphIndex = -1;

        internal const float SERVICE_TITLE_HEIGHT = 60f;
        internal const float SERVICE_MARGIN = 1f;

        internal static TaskCenter Center()
        {
            if (null == TaskCenter.instance)
            {
                throw new MissingComponentException("TaskCenter component is missing!");
            }
            return TaskCenter.instance;
        }

        private static TaskCenter instance = null;
    }
}
