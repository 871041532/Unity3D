// -----------------------------------------------------------------
// File:    AsyncOperationTask.cs
// Author:  mouguangyi
// Date:    2016.06.29
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// @details ����AsyncOperation���첽��������
    /// </summary>
    /// <typeparam name="T">�̳���AsyncOperation�Ĳ�����</typeparam>
    public class AsyncOperationTask<T> : AsyncTask where T : AsyncOperation
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="call">û�д�����������з��ز������첽�����塣</param>
        /// <param name="canRunOnThread">�Ƿ������������߳��ϡ�</param>
        public AsyncOperationTask(Func<T> call, bool canRunOnThread) : base(canRunOnThread)
        {
            Result = operation = call();
        }

        /// <summary>
        /// �Ƿ���ɡ�
        /// </summary>
        /// <returns>true��ʾ��ɣ�false��ʾδ��ɡ�</returns>
        protected override bool IsDone()
        {
            return this.operation.isDone;
        }

        private T operation = null;
    }
}