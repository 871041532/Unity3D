// -----------------------------------------------------------------
// File:    ServicesTask.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details 获取多个Service的异步任务。返回的Result是一个IService[]，其中的Service
    /// 顺序和传入的Service的Id在string[]中的顺序一致。
    /// </summary>
    public sealed class ServicesTask : AsyncTask
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceIds">Service的Id数组集合。</param>
        public ServicesTask(string[] serviceIds) : base(true)
        {
            this.serviceIds = serviceIds;
            Result = this.services = new IService[this.serviceIds.Length];
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            bool done = true;
            for (var i = 0; i < this.services.Length; ++i) {
                if (null == this.services[i]) {
                    done = false;
                    this.services[i] = this.center._FindService(this.serviceIds[i]);
                }
            }

            return done;
        }

        private string[] serviceIds = null;
        private IService[] services = null;
    }
}
