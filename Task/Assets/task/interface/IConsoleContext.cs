
using System.Net;

namespace GameFramework
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