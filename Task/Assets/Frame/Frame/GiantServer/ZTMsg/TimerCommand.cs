// -----------------------------------------------------------------
// File:    TimerCommand.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace GameBox.Service.GiantFreeServer
{
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    //网关向用户发送游戏时间
    struct GameTimeTimerCmd
    {
        public static byte byCmd = 2;
        public static byte byParam = 1;

        public UInt64 gameTime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    //网关向用户请求时间
    struct RequstUserGameTimeCmd
    {
        public static byte byCmd = 2;
        public static byte byParam = 2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    //用户向网关发送当前游戏时间
    struct UserResponseGameTimeCmd
    {
        public static byte byCmd = 2;
        public static byte byParam = 3;

        public uint userTmpID;
        public UInt64 gameTime;
    }
}
