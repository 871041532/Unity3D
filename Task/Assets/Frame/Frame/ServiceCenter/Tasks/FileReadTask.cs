// -----------------------------------------------------------------
// File:    FileReadTask.cs
// Author:  mouguangyi
// Date:    2016.07.19
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.IO;

namespace GameBox.Framework
{
    /// <summary>
    /// @details �ļ���ȡ�첽����
    /// </summary>
    public sealed class FileReadTask : AsyncTask
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="filePath">�ļ�·����</param>
        public FileReadTask(string filePath) : base(true)
        {
            this.filePath = filePath;
            this.stream = null;
            this.bytes = null;
            this.completed = false;
        }

        /// <summary>
        /// �Ƿ���ɡ�
        /// </summary>
        /// <returns>true��ʾ��ɣ�false��ʾδ��ɡ�</returns>
        protected override bool IsDone()
        {
            if (!File.Exists(this.filePath)) {
                this.completed = true;
            } else if (null == this.stream) {
                try {
                    this.stream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    this.bytes = new byte[this.stream.Length];
                    this.stream.BeginRead(this.bytes, 0, this.bytes.Length, new AsyncCallback(_OnReadComplete), null);
                } catch (Exception e) {
                    AnyLogger.X(e);
                    this.stream = null;
                }
            }

            return this.completed;
        }

        private void _OnReadComplete(IAsyncResult result)
        {
            Result = this.bytes;
            this.completed = true;
        }

        private string filePath = null;
        private FileStream stream = null;
        private byte[] bytes = null;
        private bool completed = false;
    }
}