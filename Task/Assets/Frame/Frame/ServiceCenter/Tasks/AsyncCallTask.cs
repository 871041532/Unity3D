// -----------------------------------------------------------------
// File:    AsyncCallTask.cs
// Author:  mouguangyi
// Date:    2016.06.29
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Framework
{
    enum CallState
    {
        NOTCALLED = 0,
        CALLING = 1,
        CALLED = 2,
    }

    /// <summary>
    /// @details 异步调用任务。
    /// </summary>
    public class AsyncCallTask : AsyncTask
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="call">没有传入参数和返回参数的调用体。</param>
        /// <param name="canRunOnThread">是否能运行在子线程上。</param>
        public AsyncCallTask(Action call, bool canRunOnThread) : base(canRunOnThread)
        {
            this.call = call;
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            switch (this.state) {
            case CallState.NOTCALLED:
                this.call();
                this.state = CallState.CALLING;
                break;
            default:
                this.state = CallState.CALLED;
                break;
            }

            return (CallState.CALLED == this.state);
        }

        private Action call = null;
        private CallState state = CallState.NOTCALLED;
    }

    /// <summary>
    /// @details 带有返回值的异步调用任务。
    /// </summary>
    /// <typeparam name="TResult">返回值类型。</typeparam>
    public class AsyncCallWithResultTask<TResult> : AsyncTask
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="call">没有传入参数，但有返回参数的调用体。</param>
        /// <param name="canRunOnThread">是否能运行在子线程上。</param>
        public AsyncCallWithResultTask(Func<TResult> call, bool canRunOnThread) : base(canRunOnThread)
        {
            this.call = call;
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            switch (this.state) {
            case CallState.NOTCALLED:
                Result = this.call();
                this.state = CallState.CALLING;
                break;
            default:
                this.state = CallState.CALLED;
                break;
            }

            return (CallState.CALLED == this.state);
        }

        private Func<TResult> call = null;
        private CallState state = CallState.NOTCALLED;
    }
}