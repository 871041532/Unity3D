// -----------------------------------------------------------------
// File:    GameTime.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.GiantFreeServer
{
    class GameTime
    {
        private LoginModule loginModule = null;
        private uint m_dwClientTime = 0;
        private ulong m_dwServerTime = 0;
        private ulong m_qwClientMsec = 0;
        private DateTime startTime = new DateTime(1970, 1, 1).ToLocalTime();

        public GameTime(LoginModule loginModule)
        {
            this.loginModule = loginModule;
        }

        private uint ConvertDateTimeInt(DateTime time)
        {
            TimeSpan span = (TimeSpan)(time - this.startTime);
            return (uint)span.TotalSeconds;
        }

        public uint GetIntervalMsecond()
        {
            ulong nowMsecond = this.GetNowMsecond();
            if (nowMsecond >= this.m_qwClientMsec) {
                ulong num2 = nowMsecond - this.m_qwClientMsec;
                return (uint)num2;
            }
            return 0;
        }

        public ulong GetNowMsecond()
        {
            TimeSpan span = (TimeSpan)(DateTime.Now - this.startTime);
            return (ulong)span.TotalMilliseconds;
        }

        public uint GetNowSecond()
        {
            return this.ConvertDateTimeInt(DateTime.Now);
        }

        public ulong GetServerTime()
        {
            uint nowSecond = this.GetNowSecond();
            if (nowSecond >= this.m_dwClientTime) {
                return ((this.m_dwServerTime + nowSecond) - this.m_dwClientTime);
            }
            return this.m_dwServerTime;
        }

        public void Init()
        {
            this.loginModule._RegMessageCallback(GameTimeTimerCmd.byCmd, GameTimeTimerCmd.byParam, OnServerTimeInit);
            this.loginModule._RegMessageCallback(RequstUserGameTimeCmd.byCmd, RequstUserGameTimeCmd.byParam, OnServerTimeReq);
        }

        private void OnServerTimeInit(byte[] data)
        {
            GameTimeTimerCmd cmd = (GameTimeTimerCmd)CmdSerializer.BytesToStruct(data, typeof(GameTimeTimerCmd));
            this.m_dwServerTime = cmd.gameTime;
            this.m_dwClientTime = this.GetNowSecond();
        }

        private void OnServerTimeReq(byte[] data)
        {
            this.UpdateNowMsecond();
            this.SendUserGameTime();
        }

        private void SendUserGameTime()
        {
            UserResponseGameTimeCmd cmd;
            ulong serverTime = this.GetServerTime();
            cmd.userTmpID = this.loginModule.UserTmpID;
            cmd.gameTime = serverTime;
            var server = ServiceCenter.GetService<IGiantFreeServer>();
            server.SendMessage(UserResponseGameTimeCmd.byCmd, UserResponseGameTimeCmd.byParam, CmdSerializer.StructToBytes(cmd));
        }

        private void UpdateNowMsecond()
        {
            if (this.m_qwClientMsec == 0) {
                this.m_qwClientMsec = this.GetNowMsecond();
            }
        }
    }
}


