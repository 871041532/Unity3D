
using System.Collections.Generic;
using System.Linq;

namespace GameFramework
{
    /// <summary>
    /// @details 同时开始一系列任务，并等待所有任务完成的异步任务。
    /// </summary>
    public sealed class WaitAllTask : AsyncTask
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tasks"></param>
        public WaitAllTask(IEnumerable<AsyncTask> tasks) : base(false)
        {
            this.leftNumber = tasks.Count();
            foreach (var task in tasks)
            {
                task.Start().Continue(_ =>
                {
                    --this.leftNumber;
                    return null;
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            return (0 == this.leftNumber);
        }

        private int leftNumber = 0;
    }
}