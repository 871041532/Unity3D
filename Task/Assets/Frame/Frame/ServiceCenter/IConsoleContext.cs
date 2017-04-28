// -----------------------------------------------------------------
// File:    IConsoleContext.cs
// Author:  mouguangyi
// Date:    2017.01.22
// Description:
//      
// -----------------------------------------------------------------
using System.Net;

namespace GameBox.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConsoleContext
    {
        /// <summary>
        /// 
        /// </summary>
        HttpListenerRequest Request { get; }

        /// <summary>
        /// 
        /// </summary>
        HttpListenerResponse Response { get; }
    }
}