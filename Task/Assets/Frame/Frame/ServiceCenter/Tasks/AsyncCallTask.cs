// -----------------------------------------------------------------
// File:    AsyncCallTask.cs
// Author:  mouguangyi
// Date:    2016.06.29
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Framework
{
    enum CallState
    {
        NOTCALLED = 0,
        CALLING = 1,
        CALLED = 2,
    }

    /// <summary>
    /// @details �첽��������
    /// </summary>
    public class AsyncCallTask : AsyncTask
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="call">û�д�������ͷ��ز����ĵ����塣</param>
        /// <param name="canRunOnThread">�Ƿ������������߳��ϡ�</param>
        public AsyncCallTask(Action call, bool canRunOnThread) : base(canRunOnThread)
        {
            this.call = call;
        }

        /// <summary>
        /// �Ƿ���ɡ�
        /// </summary>
        /// <returns>true��ʾ��ɣ�false��ʾδ��ɡ�</returns>
        protected override bool IsDone()
        {
            switch (this.state) {
            case CallState.NOTCALLED:
                this.call();
                this.state = CallState.CALLING;
                break;
            default:
                this.state = CallState.CALLED;
                break;
            }

            return (CallState.CALLED == this.state);
        }

        private Action call = null;
        private CallState state = CallState.NOTCALLED;
    }

    /// <summary>
    /// @details ���з���ֵ���첽��������
    /// </summary>
    /// <typeparam name="TResult">����ֵ���͡�</typeparam>
    public class AsyncCallWithResultTask<TResult> : AsyncTask
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="call">û�д�����������з��ز����ĵ����塣</param>
        /// <param name="canRunOnThread">�Ƿ������������߳��ϡ�</param>
        public AsyncCallWithResultTask(Func<TResult> call, bool canRunOnThread) : base(canRunOnThread)
        {
            this.call = call;
        }

        /// <summary>
        /// �Ƿ���ɡ�
        /// </summary>
        /// <returns>true��ʾ��ɣ�false��ʾδ��ɡ�</returns>
        protected override bool IsDone()
        {
            switch (this.state) {
            case CallState.NOTCALLED:
                Result = this.call();
                this.state = CallState.CALLING;
                break;
            default:
                this.state = CallState.CALLED;
                break;
            }

            return (CallState.CALLED == this.state);
        }

        private Func<TResult> call = null;
        private CallState state = CallState.NOTCALLED;
    }
}