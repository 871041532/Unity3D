// -----------------------------------------------------------------
// File:    GameServerLogin.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;
using GameBox.Framework;
using System.Text;

namespace GameBox.Service.GiantFreeServer
{
    class GameServerLogin
    {
        public GameServerLogin(LoginModule loginModule)
        {
            this.loginModule = loginModule;
        }

        public void ConnectGameServer(string ip, int port)
        {
            Logger<IGiantFreeServer>.L("正在连接GameServer...");
            this.loginModule._ConnectServer(ServerType.ServerType_GameWay, ip, port, OnConnectGateWayServerSuccess, OnConnectGateWayServerFailed);
        }

        private void OnConnectGateWayServerSuccess()
        {
            Logger<IGiantFreeServer>.L("连接Game Server成功");

            this.loginModule.CheckGameVersion();

            this.SendGateWayLogin();

            this.loginModule._OnLoginGameSuccess();
        }

        private void OnConnectGateWayServerFailed()
        {
        }

        public void SendGateWayLogin()
        {
            Logger<IGiantFreeServer>.L("正在登录GameServer...");

            stPasswordLoginCmd cmd;
            cmd.strName = new byte[48];

            cmd.userID = this.loginModule.UserID;
            cmd.loginTmpID = this.loginModule.UserTmpID;
            byte[] tmp = Encoding.UTF8.GetBytes(this.loginModule.Account);
            Array.Copy(tmp, cmd.strName, tmp.Length);
            cmd.strPassword = "";

            this.loginModule._SendMessage(stPasswordLoginCmd.byCmd, stPasswordLoginCmd.byParam, CmdSerializer.StructToBytes(cmd));
        }

        private LoginModule loginModule = null;
    }
}