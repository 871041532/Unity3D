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
    /// @details RPC�������͡�
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// �ͻ����������͸���������
        /// </summary>
        PULL,

        /// <summary>
        /// �������������͸��ͻ��ˡ�
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
    /// @details Giant light server RPC�������
    /// </summary>
    public interface IGiantLightProxy : IDisposable
    {
        /// <summary>
        /// ע�᷽����Ӧ�������ͷ���ֵ���͡�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="handler"></param>
        void CommonRegister<T>(string method, CommonCallDelegate handler);

        /// <summary>
        /// ע�᷽����Ӧ�������ͷ���ֵ���͡�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="handler"></param>
        void Register<T>(string method, CallDelegate<T> handler);

        /// <summary>
        /// ����RPC������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="content"></param>
        void Call<T>(string method, T content);
    }
}