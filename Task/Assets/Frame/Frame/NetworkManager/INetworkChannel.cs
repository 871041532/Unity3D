// -----------------------------------------------------------------
// File:    INetworkChannel.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.NetworkManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public delegate void OnChannelStateChange(string state);

    /// <summary>
    /// 
    /// </summary>
    public interface INetworkChannel : IDisposable
    {
    }
}