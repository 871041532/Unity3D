// -----------------------------------------------------------------
// File:    WaitAllTask.cs
// Author:  mouguangyi
// Date:    2017.01.05
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;

namespace GameBox.Framework
{
    /// <summary>
    /// @details ͬʱ��ʼһϵ�����񣬲��ȴ�����������ɵ��첽����
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
            foreach (var task in tasks) {
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