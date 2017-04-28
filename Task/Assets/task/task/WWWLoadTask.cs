using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// @details 通过WWW加载的异步任务。
    /// </summary>
    public sealed class WWWLoadTask : AsyncTask
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="url">访问的URL。</param>
        /// <param name="timeout">超时时间，默认为永不超时。</param>
        public WWWLoadTask(string url, float timeout = Mathf.Infinity) : base(false)
        {
            Result = this.w = new WWW(url);
            this.timeout = timeout;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="url">访问的URL。</param>
        /// <param name="data">POST数据。</param>
        /// <param name="timeout">超时时间，默认为永不超时。</param>
        public WWWLoadTask(string url, byte[] data, float timeout = Mathf.Infinity) : base(false)
        {
            Result = this.w = new WWW(url, data);
            this.timeout = timeout;
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            if (this.w.isDone)
            {
                return true;
            }

            this.timeout -= Time.deltaTime;
            if (this.timeout <= 0f)
            {
                this.w.Dispose(); // NOTE: Calling WWW.Dispose() will hang app on real device in Unity version < 5.3
                Result = this.w = null;
                return true;
            }

            return false;
        }

        private WWW w = null;
        private float timeout = 0;
    }
}
