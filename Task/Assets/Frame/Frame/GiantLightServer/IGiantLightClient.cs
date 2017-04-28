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
    /// @details �ͻ�����Ҫʵ�ֵ���IGiantLightServer�ԽӵĽӿڡ�
    /// 
    /// @section example ����
    /// @code{.cs}
    /// class Client : IGiantLightClient
    /// {
    ///   public void OnDisconnect()
    ///   {
    ///     // ������������Ӷ�ʧ
    ///   }
    ///   
    ///   public bool PushRequest(uint id, string service, string method, byte[] content)
    ///   {
    ///     // ������������͹��������󣬷���true��ʾ�Ѿ�������֮����һ֡�������͹���
    ///   }
    ///   
    ///   public bool PushResponse(uint id, byte[] content)
    ///   {
    ///     // ������������͹�������Ӧ������true��ʾ�Ѿ�������֮����һ֡�������͹���
    ///   }
    /// }
    /// @endcode
    /// </summary>
    public interface IGiantLightClient
    {
        /// <summary>
        /// ���ͷ���������ʱ��IGiantLightServer��ص��÷���֪ͨ�ͻ��ˡ�
        /// </summary>
        void OnConnect();

        /// <summary>
        /// ���ͷ�����ʧȥ����ʱ��IGiantLightServer��ص��÷���֪ͨ�ͻ��ˡ�
        /// </summary>
        void OnDisconnect();

        /// <summary>
        /// Giant Server���͹�����Request����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns>true��ʾ�����ˣ�false��ʾû�д���֮��IGiantLightServer�����ٴ�����</returns>
        bool PushRequest(uint id, string service, string method, byte[] content);

        /// <summary>
        /// Giant Server���͹�����Response����
        /// </summary>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns>true��ʾ�����ˣ�false��ʾû�д���֮��IGiantLightServer�����ٴ�����</returns>
        bool PushResponse(string service, string method, byte[] content);
    }
}