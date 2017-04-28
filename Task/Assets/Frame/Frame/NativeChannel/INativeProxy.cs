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
    /// Native层提供的代理接口，可实现Managed层调用Native方法。
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