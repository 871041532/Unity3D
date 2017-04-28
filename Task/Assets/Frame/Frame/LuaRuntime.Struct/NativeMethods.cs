// -----------------------------------------------------------------
// File:    NativeMethods.cs
// Author:  mouguangyi
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace GameBox.Service.LuaRuntime.Struct
{
    class NativeMethods
    {
#if UNITY_IOS
        const string STRUCTLIBNAME = "__Internal";
#else
        const string STRUCTLIBNAME = "luastruct";
#endif

        [DllImport(STRUCTLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaopen_struct")]
        internal static extern int LuaOpenStruct(IntPtr luaState);
    }
}