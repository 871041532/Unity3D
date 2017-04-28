// -----------------------------------------------------------------
// File:    IGiantLightProxy.cs
// Author:  mouguangyi
// Date:    2017.03.03
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.GiantLightServer
{
    /// <summary>
    /// @details RPC服务类型。
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// 客户端主动发送给服务器。
        /// </summary>
        PULL,

        /// <summary>
        /// 服务器主动发送给客户端。
        /// </summary>
        PUSH,
    }

    /// <summary>
    /// @details
    /// </summary>
    /// <param name="method"></param>
    /// <param name="content"></param>
    public delegate void CommonCallDelegate(string method, object content);

    /// <summary>
    /// @details
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="method"></param>
    /// <param name="content"></param>
    public delegate void CallDelegate<T>(string method, T content);

    /// <summary>
    /// @details Giant light server RPC服务代理。
    /// </summary>
    public interface IGiantLightProxy : IDisposable
    {
        /// <summary>
        /// 注册方法对应处理函数和返回值类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="handler"></param>
        void CommonRegister<T>(string method, CommonCallDelegate handler);

        /// <summary>
        /// 注册方法对应处理函数和返回值类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="handler"></param>
        void Register<T>(string method, CallDelegate<T> handler);

        /// <summary>
        /// 调用RPC方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="content"></param>
        void Call<T>(string method, T content);
    }
}