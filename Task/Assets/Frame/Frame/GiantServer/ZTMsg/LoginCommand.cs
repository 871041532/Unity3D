// -----------------------------------------------------------------
// File:    LoginCommand.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace GameBox.Service.GiantFreeServer
{
    //Request Client IP Cmd
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stRequestClientIPCmd
    {
        public static byte byCmd = 104;
        public static byte byParam = 15;
    }

    //检查客户端版本
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stCheckVerisonCmd
    {
        public static byte byCmd = 104;
        public static byte byParam = 120;

        public uint data;
        public uint version;
    }

    //返回客户端ip
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stResponceClientIPCmd
    {
        public static byte byCmd = 104;
        public static byte byParam = 16;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] ip;   // Use byte array to save ip info
    }

    //请求登录
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stRequestLoginCmd
    {
        public static byte byCmd = 104;
        public static byte byParam = 2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string strName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33, ArraySubType = UnmanagedType.U1)]
        public byte[] strPassword;
        public ushort game;
        public ushort zone;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string jpegPassword;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string macAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
        public string uuid;
        public ushort wdNetType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string passpodPwd;
        public ushort userType;
        public uint tempAccid;
        public ushort data1;
        public ushort data2;
        public ushort data3;
        public ushort data4;
    }

    //登录失败
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stLoginFailedCmd
    {
        public static byte byCmd = 104;
        public static byte byParam = 3;

        public byte retCode;
    }

    //登录成功
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stLoginSuccessCmd
    {
        public static byte byCmd = 104;
        public static byte byParam = 4;

        public uint dwUserID;
        public uint loinTmpID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string gameserIP;
        public ushort port;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.U1)]
        public byte[] key;
        public uint state;
    }

    //登录网关发送账号密码
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stPasswordLoginCmd
    {
        public static byte byCmd = 104;
        public static byte byParam = 5;

        public uint loginTmpID;
        public uint userID;
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        //public string strName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.U1)]
        public byte[] strName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string strPassword;
    }
}

