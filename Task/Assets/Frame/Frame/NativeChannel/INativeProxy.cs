// -----------------------------------------------------------------
// File:    INativeProxy.cs
// Author:  mouguangyi
// Date:    2017.02.21
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.NativeChannel
{
    /// <summary>
    /// Native���ṩ�Ĵ���ӿڣ���ʵ��Managed�����Native������
    /// </summary>
    public interface INativeProxy : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        void Call(string method, params object[] args);

        /// <summary>
        /// 
        /// </summary>
        void Disconnect();
    }
}