// -----------------------------------------------------------------
// File:    IHttpChannel.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System.Collections.Generic;

namespace GameBox.Service.NetworkManager
{
    /// <summary>
    /// HTTP通信通道。
    /// </summary>
    public interface IHttpChannel : INetworkChannel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        AsyncTask Request(string url, string method = "GET", IDictionary<string, object> data = null);
    }
}