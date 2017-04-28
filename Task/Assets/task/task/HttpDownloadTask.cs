
using System;
using System.Net;

namespace GameFramework
{
    /// <summary>
    /// @details HTTP异步下载任务。
    /// </summary>
    public sealed class HttpDownloadTask : AsyncTask
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public HttpDownloadTask(string url) : base(true)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            this.client = new WebClient();
            this.client.DownloadProgressChanged += _OnDownloadProgressChanged;
            this.client.DownloadDataCompleted += _OnDownloadDataCompleted;
            this.client.DownloadDataAsync(new Uri(url));
        }

        /// <summary>
        /// 
        /// </summary>
        public long DownloadedSize
        {
            get
            {
                return this.downloadedSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            return this.completed;
        }

        private void _OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs args)
        {
            this.downloadedSize += args.BytesReceived;
        }

        private void _OnDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs args)
        {
            Result = args.Result;
            this.completed = true;
        }

        private WebClient client = null;
        private long downloadedSize = 0;
        private bool completed = false;
    }
}