// -----------------------------------------------------------------
// File:    ISocketChannel.cs
// Author:  mouguangyi
// Date:    2017.03.15
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.ByteStorage;
using System;
using System.Net;

namespace GameBox.Service.NetworkManager
{
    /// <summary>
    /// Socket通信通道。
    /// </summary>
    public interface ISocketChannel : INetworkChannel
    {
        /// <summary>
        /// 接收缓冲区大小，默认为256k。
        /// </summary>
        int BufferSize { set; }

        /// <summary>
        /// 
        /// </summary>
        OnChannelStateChange OnChannelStateChange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        void Connect(string ip, int port);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        void Connect(IPEndPoint endPoint);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        void Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        void Send(byte[] data, int size);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void Send(IByteArray data);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true表示还有未处理的数据包；false表示已经没有未处理的数据包。</returns>
        bool Receive(Action<IByteArray> handler);
    }
}