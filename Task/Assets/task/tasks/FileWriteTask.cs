using System;
using System.IO;

namespace GameFramework
{
    /// <summary>
    /// @details 文件写入模式。
    /// </summary>
    public enum FileWriteMode
    {
        /// <summary>
        /// 覆盖。
        /// </summary>
        OVERRIDE = SeekOrigin.Begin,

        /// <summary>
        /// 增量。
        /// </summary>
        APPEND = SeekOrigin.End,
    }

    /// <summary>
    /// @details 文件写入异步任务。
    /// </summary>
    public sealed class FileWriteTask : AsyncTask
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <param name="bytes">写入的内容。</param>
        /// <param name="mode">文件写入模式<see cref="FileWriteMode"/></param>
        public FileWriteTask(string filePath, byte[] bytes, FileWriteMode mode = FileWriteMode.OVERRIDE) : base(true)
        {
            this.filePath = filePath;
            this.bytes = bytes;
            this.mode = mode;
            this.stream = null;
            this.completed = false;
        }

        /// <summary>
        /// 是否完成。
        /// </summary>
        /// <returns>true表示完成；false表示未完成。</returns>
        protected override bool IsDone()
        {
            if (null == this.stream)
            {
                try
                {
                    var info = new FileInfo(this.filePath);
                    if (!Directory.Exists(info.DirectoryName))
                    {
                        Directory.CreateDirectory(info.DirectoryName);
                    }

                    this.stream = new FileStream(this.filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    this.stream.Seek(0, (SeekOrigin)this.mode);
                    this.stream.BeginWrite(this.bytes, 0, this.bytes.Length, new AsyncCallback(_OnWriteComplete), null);
                }
                catch (Exception e)
                {
                    this.stream = null;
                }
            }

            return this.completed;
        }

        private void _OnWriteComplete(IAsyncResult result)
        {
            if (FileWriteMode.OVERRIDE == this.mode)
            {
                this.stream.SetLength(this.bytes.Length);
            }
            this.stream.Close();
            this.completed = true;
        }

        private string filePath = null;
        private byte[] bytes = null;
        private FileWriteMode mode = FileWriteMode.OVERRIDE;
        private FileStream stream = null;
        private bool completed = false;
    }
}