// -----------------------------------------------------------------
// File:    IGiantFreeClient.cs
// Author:  mouguangyi
// Date:    2016.08.09
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantFreeServer
{
    /// <summary>
    /// @details ���˶��η������ͻ��˽���ӿڡ�
    /// </summary>
    public interface IGiantFreeClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="param"></param>
        /// <param name="data"></param>
        void HandleMessage(byte command, byte param, byte[] data);

        /// <summary>
        /// 
        /// </summary>
        void OnDisconnect();
    }
}