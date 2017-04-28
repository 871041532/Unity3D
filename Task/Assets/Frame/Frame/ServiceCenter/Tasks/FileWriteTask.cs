// -----------------------------------------------------------------
// File:    FileWriteTask.cs
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
    /// @details �ļ�д��ģʽ��
    /// </summary>
    public enum FileWriteMode
    {
        /// <summary>
        /// ���ǡ�
        /// </summary>
        OVERRIDE = SeekOrigin.Begin,

        /// <summary>
        /// ������
        /// </summary>
        APPEND = SeekOrigin.End,
    }

    /// <summary>
    /// @details �ļ�д���첽����
    /// </summary>
    public sealed class FileWriteTask : AsyncTask
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="filePath">�ļ�·����</param>
        /// <param name="bytes">д������ݡ�</param>
        /// <param name="mode">�ļ�д��ģʽ<see cref="FileWriteMode"/></param>
        public FileWriteTask(string filePath, byte[] bytes, FileWriteMode mode = FileWriteMode.OVERRIDE) : base(true)
        {
            this.filePath = filePath;
            this.bytes = bytes;
            this.mode = mode;
            this.stream = null;
            this.completed = false;
        }

        /// <summary>
        /// �Ƿ���ɡ�
        /// </summary>
        /// <returns>true��ʾ��ɣ�false��ʾδ��ɡ�</returns>
        protected override bool IsDone()
        {
            if (null == this.stream) {
                try {
                    var info = new FileInfo(this.filePath);
                    if (!Directory.Exists(info.DirectoryName)) {
                        Directory.CreateDirectory(info.DirectoryName);
                    }

                    this.stream = new FileStream(this.filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    this.stream.Seek(0, (SeekOrigin)this.mode);
                    this.stream.BeginWrite(this.bytes, 0, this.bytes.Length, new AsyncCallback(_OnWriteComplete), null);
                } catch (Exception e) {
                    AnyLogger.X(e);
                    this.stream = null;
                }
            }

            return this.completed;
        }

        private void _OnWriteComplete(IAsyncResult result)
        {
            if (FileWriteMode.OVERRIDE == this.mode) {
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