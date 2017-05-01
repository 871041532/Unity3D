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
    /// @details �������������������񡣷�װ�˾��˵��������������ļ��ɷ�����
    /// 
    /// @section example ����
    /// @code{.cs}
    /// // ����ʵ��IGiantLightClient�ӿ�
    /// class Client : IGiantLightClient
    /// {
    ///   ...
    /// }
    /// 
    /// var server = TaskCenter.GetService<IGiantLightServer>();
    /// server.Connect("xx.xx.xx.xx", 8000, new Client());
    /// ...
    /// 
    /// // ��������
    /// var login = new LoginProtoObject(); // LoginProtoObject��һ��protocol-buffers����
    /// server.SendRequest(0, "[service]", "[method]", ByteConverter.ProtoBufToBytes<LoginProtoObject>(login));
    /// ...
    /// 
    /// // ���ͻظ�
    /// var result = new ResultProtoObject();   // ResultProtoObject��һ��protocol-buffers����
    /// server.SendResponse(0, ByteConverter.ProtoBufToBytes<ResultProtoObject>(result));
    /// 
    /// @endcode
    /// 
    /// </summary>
    public interface IGiantLightServer : IService
    {
        /// <summary>
        /// �Ƿ����ӡ�
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// ����Giant light server��������
        /// </summary>
        /// <param name="ip">IP��ַ</param>
        /// <param name="port">�˿�</param>
        /// <param name="client">ʵ�ֵ�IGiantLightClient�ӿ�ʵ��</param>
        void Connect(string ip, int port, IGiantLightClient client);

        /// <summary>
        /// �Ͽ���Giant light server�����ӡ�
        /// </summary>
        void Disconnect();

        /// <summary>
        /// ����Request����
        /// </summary>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        void SendRequest(string service, string method, byte[] content);

        /// <summary>
        /// ����Response����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="content"></param>
        void SendResponse(uint id, byte[] content);

        /// <summary>
        /// ���ݷ����������ʹ���IGiantLightProxy��
        /// </summary>
        /// <param name="service"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IGiantLightProxy CreateProxy(string service, ServiceType type);

        /// <summary>
        /// ���ݷ���������IGiantLightProxy��
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        IGiantLightProxy FindProxy(string service);
    }
}