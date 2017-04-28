// -----------------------------------------------------------------
// File:    INetworkManager.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.NetworkManager
{
    /// <summary>
    /// 网络管理服务。封装了所有网络底层的各种通信服务。
    /// </summary>
    public interface INetworkManager : IService
    {
        /// <summary>
        /// 根据通信类型创建通信的通道。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        INetworkChannel Create(string type);
    }
}