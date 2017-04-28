// -----------------------------------------------------------------
// File:    FirServerLogin.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using GameBox.Framework;

namespace GameBox.Service.GiantFreeServer
{
    class FirServerLogin
    {
        private LoginModule loginModule = null;
        private Dictionary<int, string> loginErrorDict;

        public FirServerLogin(LoginModule loginModule)
        {
            this.loginModule = loginModule;
            this.loginModule._RegMessageCallback(stResponceClientIPCmd.byCmd, stResponceClientIPCmd.byParam, OnResponseClientIP);
            this.loginModule._RegMessageCallback(stLoginFailedCmd.byCmd, stLoginFailedCmd.byParam, OnLoginFirFailed);
            this.loginModule._RegMessageCallback(stLoginSuccessCmd.byCmd, stLoginSuccessCmd.byParam, OnLoginFirSuccess);

            InitErrorDict();
        }

        public void ConnectFirServer(string ip, int port)
        {
            Logger<IGiantFreeServer>.L("正在连接Fir Server...");
            this.loginModule._ConnectServer(ServerType.ServerType_Fir, ip, port, OnConnectFirSuccess, OnConnectFirFailed);
        }

        private void OnResponseClientIP(byte[] data)
        {
            stResponceClientIPCmd cmd = (stResponceClientIPCmd)CmdSerializer.BytesToStruct(data, typeof(stResponceClientIPCmd));
            {
                if (cmd.ip == null) {
                    Logger<IGiantFreeServer>.E("IP地址为空！");
                    return;
                }

                Array.Copy(cmd.ip, this.loginModule.szKeyIP, 16);
                this.LoginFirServer();
            }
        }

        public void LoginFirServer()
        {
            Logger<IGiantFreeServer>.L("正在登录Fir Server...");

            stRequestLoginCmd loginCmd = new stRequestLoginCmd();
            loginCmd.strName = this.loginModule.Account;

            byte len = this.loginModule.Password[0];
            byte[] szPass = new byte[34];
            for (int i = 0; i < (len + 1); i++) {
                szPass[i] = this.loginModule.Password[i];
            }
            this.UseIPEncry(ref szPass, len + 1);

            loginCmd.strPassword = new byte[33];
            Array.Copy(szPass, loginCmd.strPassword, 33);
            loginCmd.game = this.loginModule.GameType;
            loginCmd.zone = this.loginModule.ZoneID;
            loginCmd.macAddress = "74D02BC4CE2B";
            loginCmd.uuid = "))L1?o1?L1????z/ 2?";
            loginCmd.wdNetType = 0;

            this.loginModule._SendMessage(stRequestLoginCmd.byCmd, stRequestLoginCmd.byParam, CmdSerializer.StructToBytes(loginCmd));
        }

        private void UseIPEncry(ref byte[] pszSrc, int iNum)
        {
            byte nKey = (byte)this.loginModule.szKeyIP.Length;
            byte rKey = 0;
            for (int i = 0; i < iNum; ++i) {
                pszSrc[i] ^= this.loginModule.szKeyIP[rKey];
                pszSrc[i]++;
                if (++rKey >= nKey)
                    rKey = 0;
            }
        }

        public void OnLoginFirSuccess(byte[] data)
        {
            Logger<IGiantFreeServer>.L("登录Fir Server成功");

            stLoginSuccessCmd cmd = (stLoginSuccessCmd)CmdSerializer.BytesToStruct(data, typeof(stLoginSuccessCmd));
            {
                this.loginModule.GateWayIP = cmd.gameserIP;
                this.loginModule.GateWayPort = cmd.port;
                this.loginModule.UserID = cmd.dwUserID;
                this.loginModule.UserTmpID = cmd.loinTmpID;
                Array.Copy(cmd.key, this.loginModule.LoginKey, 256);
                byte index = this.loginModule.LoginKey[58];
                DESEncrypt.ResetDESKey(this.loginModule.LoginKey, index);
                uint desKey = (uint)this.loginModule.LoginKey[index + 2];
                DESEncryptorFix.SetDESEcryptMask(desKey);
                this.loginModule.LoginGameServer();
            }
        }

        public void OnLoginFirFailed(byte[] data)
        {
            this.loginModule._NotifyClient(stLoginFailedCmd.byCmd, stLoginFailedCmd.byParam, data);
            stLoginFailedCmd cmd = (stLoginFailedCmd)CmdSerializer.BytesToStruct(data, typeof(stLoginFailedCmd));
            {
                string str = string.Format("OnLoginFirFailed!!, ErrorID = {0}, ErrorMsg: {1}", cmd.retCode, this.LoginErrorMsg((int)cmd.retCode));
                Logger<IGiantFreeServer>.L(str);
            }
        }

        private void OnConnectFirSuccess()
        {
            Logger<IGiantFreeServer>.L("连接Fir Server成功");

            this.loginModule.CheckGameVersion();

            stRequestClientIPCmd requestIP;
            this.loginModule._SendMessage(stRequestClientIPCmd.byCmd, stRequestClientIPCmd.byParam, CmdSerializer.StructToBytes(requestIP));
        }

        public void OnConnectFirFailed()
        {

        }

        private void InitErrorDict()
        {
            this.loginErrorDict = new Dictionary<int, string>();
            this.loginErrorDict.Add(0, "未知错误");
            this.loginErrorDict.Add(1, "版本错误");
            this.loginErrorDict.Add(2, "UUID登陆方式没有实现");
            this.loginErrorDict.Add(3, "数据库出错");
            this.loginErrorDict.Add(4, "帐号密码错误");
            this.loginErrorDict.Add(5, "修改密码成功");
            this.loginErrorDict.Add(6, "ID正在被使用中");
            this.loginErrorDict.Add(7, "ID被封");
            this.loginErrorDict.Add(8, "网关服务器未开");
            this.loginErrorDict.Add(9, "用户满");
            this.loginErrorDict.Add(10, "账号已经存在");
            this.loginErrorDict.Add(11, "注册账号成功");
            this.loginErrorDict.Add(12, "角色名称重复");
            this.loginErrorDict.Add(13, "用户档案不存在");
            this.loginErrorDict.Add(14, "用户名重复");
            this.loginErrorDict.Add(15, "连接超时");
            this.loginErrorDict.Add(0x10, "计费失败");
            this.loginErrorDict.Add(0x11, "图形验证码输入错误");
            this.loginErrorDict.Add(0x12, "帐号被锁定");
            this.loginErrorDict.Add(0x13, "帐号待激活");
            this.loginErrorDict.Add(20, "新账号不允许登入旧的游戏区");
            this.loginErrorDict.Add(0x15, "登录UUID错误");
            this.loginErrorDict.Add(0x16, "角色已登录战区,不允许创建角色");
            this.loginErrorDict.Add(0x17, "跨区登陆验证失败");
            this.loginErrorDict.Add(0x18, "登录矩阵卡密码错误");
            this.loginErrorDict.Add(0x19, "提示玩家需要输入矩阵卡密码");
            this.loginErrorDict.Add(0x1a, "提示玩家矩阵卡被锁（六个小时后解锁）");
            this.loginErrorDict.Add(0x1b, "与矩阵卡验证服务器失去连接");
            this.loginErrorDict.Add(0x1c, "旧帐号不允许登陆新区");
            this.loginErrorDict.Add(0x1d, "图形验证连续错误3次,角色被锁定");
            this.loginErrorDict.Add(30, "密保密码错误");
            this.loginErrorDict.Add(0x1f, "与密保服务器失去连接");
            this.loginErrorDict.Add(0x20, "服务器繁忙");
            this.loginErrorDict.Add(0x21, "帐号被封停");
            this.loginErrorDict.Add(0x22, "图形验证连续错误9次，角色被锁定");
            this.loginErrorDict.Add(0x23, "游戏区正常维护中，原来的错误码LOGIN_RETURN_GATEWAYNOTAVAILABLE表示非正常维护");
            this.loginErrorDict.Add(0x24, "获取二维码失败");
            this.loginErrorDict.Add(0x25, "二维码服务不可用,请输入帐号密码登陆");
            this.loginErrorDict.Add(0x26, "token验证失败");
            this.loginErrorDict.Add(0x27, "TOKEN验证太快，意思说上次验证还没结束，就开始新的验证");
            this.loginErrorDict.Add(40, "TOKEN验证超时");
            this.loginErrorDict.Add(0x29, "显示后面的错误消息");
            this.loginErrorDict.Add(0x2a, "用户已经登录");
            this.loginErrorDict.Add(0x2b, "昵称含有敏感词汇");
        }

        public string LoginErrorMsg(int errorCode)
        {
            if (this.loginErrorDict.ContainsKey(errorCode))
                return this.loginErrorDict[errorCode];
            return "<<未知错误>>";
        }
    }

}
