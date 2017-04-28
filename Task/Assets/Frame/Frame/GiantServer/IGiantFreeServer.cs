// -----------------------------------------------------------------
// File:    IGiantFreeServer.cs
// Author:  fuzhun
// Date:    2016.08.05
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.GiantFreeServer
{
    /// <summary>
    /// @details ���˶��η������������ӿڡ�
    /// </summary>
    public interface IGiantFreeServer : IService
    {
        /// <summary>
        /// ��¼��;��ѷ�������
        /// </summary>
        /// <param name="name">�û�����</param>
        /// <param name="password">���롣</param>
        /// <param name="zoneId">��ID��</param>
        /// <param name="serverIP">������IP�б�</param>
        /// <param name="port">�������˿��б�</param>
        /// <param name="gameVersion">��Ϸ�汾��</param>
        /// <param name="client">�ͻ���ʵ�ֵ�IGiantFreeClient����</param>
        void Start(string name, string password, ushort zoneId, string[] serverIP, int[] port, uint gameVersion, IGiantFreeClient client);

        /// <summary>
        /// �˳���;��ѷ�������
        /// </summary>
        void Stop();

        /// <summary>
        /// ������Ϣ��
        /// </summary>
        /// <param name="command">����Ϣ�š�</param>
        /// <param name="param">С��Ϣ�š�</param>
        /// <param name="data">�����塣</param>
        void SendMessage(byte command, byte param, byte[] data);
    }
}