// -----------------------------------------------------------------
// File:    INativeChannel.cs
// Author:  mouguangyi
// Date:    2016.09.29
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.NativeChannel
{
    /// <summary>
    /// @details C#层与Native层的标准封装服务，规范交互协议，简化接入流程。
    /// </summary>
    public interface INativeChannel : IService
    {
        /// <summary>
        /// Managed层单向一次性调用Native层的函数。
        /// </summary>
        /// <param name="module"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        void Call(NativeModule module, string method, params object[] args);

        /// <summary>
        /// C#层的proxy对象与Native层的目标模块对象建立连接。
        /// </summary>
        /// <param name="targetModule"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        INativeProxy Connect<T>(NativeModule module, T proxy) where T : class;
    }
}