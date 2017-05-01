// -----------------------------------------------------------------
// File:    IGiantLightServer.cs
// Author:  mouguangyi
// Date:    2016.06.16
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.GiantLightServer
{
    /// <summary>
    /// @details 巨人轻量级服务器服务。封装了巨人的轻量级服务器的集成方案。
    /// 
    /// @section example 例子
    /// @code{.cs}
    /// // 首先实现IGiantLightClient接口
    /// class Client : IGiantLightClient
    /// {
    ///   ...
    /// }
    /// 
    /// var server = TaskCenter.GetService<IGiantLightServer>();
    /// server.Connect("xx.xx.xx.xx", 8000, new Client());
    /// ...
    /// 
    /// // 发送请求
    /// var login = new LoginProtoObject(); // LoginProtoObject是一个protocol-buffers对象
    /// server.SendRequest(0, "[service]", "[method]", ByteConverter.ProtoBufToBytes<LoginProtoObject>(login));
    /// ...
    /// 
    /// // 发送回复
    /// var result = new ResultProtoObject();   // ResultProtoObject是一个protocol-buffers对象
    /// server.SendResponse(0, ByteConverter.ProtoBufToBytes<ResultProtoObject>(result));
    /// 
    /// @endcode
    /// 
    /// </summary>
    public interface IGiantLightServer : IService
    {
        /// <summary>
        /// 是否连接。
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// 连接Giant light server服务器。
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="client">实现的IGiantLightClient接口实例</param>
        void Connect(string ip, int port, IGiantLightClient client);

        /// <summary>
        /// 断开与Giant light server的连接。
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 发送Request请求。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        void SendRequest(string service, string method, byte[] content);

        /// <summary>
        /// 发送Response请求。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="content"></param>
        void SendResponse(uint id, byte[] content);

        /// <summary>
        /// 根据服务名和类型创建IGiantLightProxy。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IGiantLightProxy CreateProxy(string service, ServiceType type);

        /// <summary>
        /// 根据服务名查找IGiantLightProxy。
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        IGiantLightProxy FindProxy(string service);
    }
}