// -----------------------------------------------------------------
// File:    LoginModule.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Text;

namespace GameBox.Service.GiantFreeServer
{
    class LoginModule
    {
        public string Account = "";
        public byte[] Password = null;
        public byte[] szKeyIP = new byte[16];
        public uint GameVersion = 3141592653;
        public ushort ZoneID = 446;
        public ushort GameType = 1;     //ZTFree is 1

        public string FirServerIP = "222.73.30.21";
        public int FirServerPort = 7000;

        public string GateWayIP;
        public ushort GateWayPort;
        public uint UserID;
        public uint UserTmpID;
        public byte[] LoginKey = new byte[256];

        private GiantFreeServer server = null;
        private FirServerLogin FirServer = null;
        private GameServerLogin GameServer = null;
        private GameTime gameTime = null;

        public LoginModule(GiantFreeServer server)
        {
            this.server = server;
            this.FirServer = new FirServerLogin(this);
            this.GameServer = new GameServerLogin(this);
            this.gameTime = new GameTime(this);
        }

        public void Login()
        {
            this.gameTime.Init();
            FirServer.ConnectFirServer(FirServerIP, FirServerPort);
        }

        public void LoginGameServer()
        {
            GameServer.ConnectGameServer(GateWayIP, GateWayPort);
        }

        public void CheckGameVersion()
        {
            stCheckVerisonCmd checkVersion;
            checkVersion.data = 0;
            checkVersion.version = GameVersion;
            var server = ServiceCenter.GetService<IGiantFreeServer>();
            server.SendMessage(stCheckVerisonCmd.byCmd, stCheckVerisonCmd.byParam, CmdSerializer.StructToBytes(checkVersion));
        }

        public void EncryptPassWord(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            this.Password = new byte[(bytes.Length * 2) + 1];
            int index = 1;
            for (int i = 0; i < bytes.Length; i++) {
                this.EncryptChar(ref this.Password, index, bytes[i]);
                index += 2;
            }
            this.Password[0] = (byte)(bytes.Length * 2);
        }

        public uint GetIntervalMsecond()
        {
            return this.gameTime.GetIntervalMsecond();
        }

        internal void _ConnectServer(ServerType type, string ip, int port, Action connectSuccess, Action connectFailed)
        {
            this.server._ConnectServer(type, ip, port, connectSuccess, connectFailed);
        }

        internal void _OnLoginGameSuccess()
        {
            this.server._OnLoginGameSuccess();
        }

        internal void _SendMessage(byte cmd, byte param, byte[] data)
        {
            this.server.SendMessage(cmd, param, data);
        }

        internal void _RegMessageCallback(byte cmd, byte param, Action<byte[]> callback)
        {
            this.server._RegMessageCallback(cmd, param, callback);
        }

        internal void _NotifyClient(byte command, byte param, byte[] data)
        {
            this.server._NotifyClient(command, param, data);
        }

        private void EncryptChar(ref byte[] pszDes, int index, byte pszSrc)
        {
            byte array = 1;
            byte[] keyData = new byte[] { 210, 0x29, 0xb6, 0x8d, 14, 0xf2, 120, 0xb2 };
            pszSrc = (byte)(pszSrc + 2);
            pszDes[index] = pszSrc;

            byte btmp = (byte)(pszDes[index + 1] >> 4);
            btmp = (byte)(btmp & 15);

            byte btmp1 = (byte)(pszDes[index] << 4);
            btmp1 = (byte)(btmp1 & 240);

            btmp = (byte)(btmp | btmp1);

            btmp1 = (byte)(pszDes[index] >> 4);
            btmp1 = (byte)(btmp1 & 15);

            byte btmp2 = (byte)(pszDes[index + 1] << 4);
            btmp2 = (byte)(btmp2 & 240);
            btmp1 |= btmp2;

            pszDes[index] = btmp1;
            pszDes[index + 1] = btmp;

            pszDes[index] ^= keyData[array];
            pszDes[index + 1] ^= keyData[array];

            btmp1 = (byte)((int)pszDes[index] & 0x0F);
            btmp2 = (byte)((int)array << 4);
            btmp1 |= btmp2;
            pszDes[index] = btmp1;
        }
    }
}
