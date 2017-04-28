// -----------------------------------------------------------------
// File:    WWWLoadFromCacheTask.cs
// Author:  mouguangyi
// Date:    2016.06.29
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// @details 通过WWW.LoadFromCacheOrDownload函数获取的异步任务。
    /// </summary>
    public sealed class WWWLoadFromCacheTask : AsyncTask
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="url">访问的URL。</param>
        /// <param name="version">版本。</param>
        public WWWLoadFromCacheTask(string url, int version) : base(true)
        {
            Result = this.w = WWW.LoadFromCacheOrDownload(url, version);
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            return this.w.isDone;
        }

        private WWW w = null;
    }
}