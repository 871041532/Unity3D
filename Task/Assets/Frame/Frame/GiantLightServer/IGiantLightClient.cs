// -----------------------------------------------------------------
// File:    IGiantLightClient.cs
// Author:  mouguangyi
// Date:    2016.07.26
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantLightServer
{
    /// <summary>
    /// @details 客户端需要实现的与IGiantLightServer对接的接口。
    /// 
    /// @section example 例子
    /// @code{.cs}
    /// class Client : IGiantLightClient
    /// {
    ///   public void OnDisconnect()
    ///   {
    ///     // 处理服务器连接丢失
    ///   }
    ///   
    ///   public bool PushRequest(uint id, string service, string method, byte[] content)
    ///   {
    ///     // 处理服务器推送过来的请求，返回true表示已经处理；反之则下一帧还会推送过来
    ///   }
    ///   
    ///   public bool PushResponse(uint id, byte[] content)
    ///   {
    ///     // 处理服务器推送过来的响应，返回true表示已经处理；反之则下一帧还会推送过来
    ///   }
    /// }
    /// @endcode
    /// </summary>
    public interface IGiantLightClient
    {
        /// <summary>
        /// 当和服务器连接时，IGiantLightServer会回调该方法通知客户端。
        /// </summary>
        void OnConnect();

        /// <summary>
        /// 当和服务器失去连接时，IGiantLightServer会回调该方法通知客户端。
        /// </summary>
        void OnDisconnect();

        /// <summary>
        /// Giant Server推送过来的Request请求。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns>true表示处理了；false表示没有处理，之后IGiantLightServer还会再次请求。</returns>
        bool PushRequest(uint id, string service, string method, byte[] content);

        /// <summary>
        /// Giant Server推送过来的Response请求。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns>true表示处理了；false表示没有处理，之后IGiantLightServer还会再次请求。</returns>
        bool PushResponse(string service, string method, byte[] content);
    }
}