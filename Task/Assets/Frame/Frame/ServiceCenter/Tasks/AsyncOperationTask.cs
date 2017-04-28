// -----------------------------------------------------------------
// File:    AsyncOperationTask.cs
// Author:  mouguangyi
// Date:    2016.06.29
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// @details 基于AsyncOperation的异步调用任务。
    /// </summary>
    /// <typeparam name="T">继承于AsyncOperation的操作。</typeparam>
    public class AsyncOperationTask<T> : AsyncTask where T : AsyncOperation
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="call">没有传入参数，但有返回参数的异步调用体。</param>
        /// <param name="canRunOnThread">是否能运行在子线程上。</param>
        public AsyncOperationTask(Func<T> call, bool canRunOnThread) : base(canRunOnThread)
        {
            Result = operation = call();
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            return this.operation.isDone;
        }

        private T operation = null;
    }
}