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
    /// @details 巨人端游服务器接入服务接口。
    /// </summary>
    public interface IGiantFreeServer : IService
    {
        /// <summary>
        /// 登录征途免费服务器。
        /// </summary>
        /// <param name="name">用户名。</param>
        /// <param name="password">密码。</param>
        /// <param name="zoneId">区ID。</param>
        /// <param name="serverIP">服务器IP列表。</param>
        /// <param name="port">服务器端口列表。</param>
        /// <param name="gameVersion">游戏版本。</param>
        /// <param name="client">客户端实现的IGiantFreeClient对象。</param>
        void Start(string name, string password, ushort zoneId, string[] serverIP, int[] port, uint gameVersion, IGiantFreeClient client);

        /// <summary>
        /// 退出征途免费服务器。
        /// </summary>
        void Stop();

        /// <summary>
        /// 发送消息。
        /// </summary>
        /// <param name="command">大消息号。</param>
        /// <param name="param">小消息号。</param>
        /// <param name="data">数据体。</param>
        void SendMessage(byte command, byte param, byte[] data);
    }
}