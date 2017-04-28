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
    /// @details ͨ��WWW.LoadFromCacheOrDownload������ȡ���첽����
    /// </summary>
    public sealed class WWWLoadFromCacheTask : AsyncTask
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="url">���ʵ�URL��</param>
        /// <param name="version">�汾��</param>
        public WWWLoadFromCacheTask(string url, int version) : base(true)
        {
            Result = this.w = WWW.LoadFromCacheOrDownload(url, version);
        }

        /// <summary>
        /// �Ƿ���ɡ�
        /// </summary>
        /// <returns>true��ʾ��ɣ�false��ʾδ��ɡ�</returns>
        protected override bool IsDone()
        {
            return this.w.isDone;
        }

        private WWW w = null;
    }
}