
namespace GameFramework
{
    /// <summary>
    /// @details 根据Service Id获取单个Service。
    /// </summary>
    public sealed class ServiceTask : AsyncTask
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="serviceId">Service的Id。</param>
        public ServiceTask(string serviceId) : base(true)
        {
            this.serviceId = serviceId;
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            Result = this.service = this.center._FindService(this.serviceId);

            return (null != this.service);
        }

        private string serviceId = null;
        private IService service = null;
    }

    /// <summary>
    /// @details 根据Service类型获取单个Service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceTask<T> : AsyncTask where T : IService
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ServiceTask() : base(true)
        { }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            Result = this.service = this.center._FindService<T>();

            return (null != this.service);
        }

        private T service = default(T);
    }
}
