// -----------------------------------------------------------------
// File:    NativeMethods.cs
// Author:  gexiaoyi
// Date:    2016.08.11
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace GameBox.Service.LuaRuntime.ProtocolBuffer
{
    static class NativeMethods
    {
#if UNITY_IOS
        const string LUAPBLIBNAME = "__Internal";
#else
        const string LUAPBLIBNAME = "luapb";
#endif

        [DllImport(LUAPBLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaopen_pb")]
        internal static extern int LuaOpenProtocolBuffer(IntPtr luaState);
    }
}