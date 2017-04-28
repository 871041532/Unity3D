// -----------------------------------------------------------------
// File:    LuaString.cs
// Author:  mouguangyi
// Date:    2016.10.14
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Runtime.InteropServices;

namespace GameBox.Service.LuaRuntime
{
    class LuaString : ILuaString
    {
        public LuaString(byte[] bytes)
        {
            if (null == bytes) {
                throw new Exception("LuaString bytes can't be null!!!");
            }

            this.bytes = bytes;
            this.length = bytes.Length;
        }

        public byte[] ToBytes()
        {
            return this.bytes;
        }

        public override string ToString()
        {
            string result = string.Empty;
            if (this.length > 0) {
                try {
                    IntPtr ptr = Marshal.AllocHGlobal(this.length);
                    {
                        Marshal.Copy(this.bytes, 0, ptr, this.length);
                        result = Marshal.PtrToStringAnsi(ptr, this.length);
                    }
                    Marshal.FreeHGlobal(ptr);
                } catch (Exception e) {
                    Logger<ILuaRuntime>.E(e);
                }
            }

            return result;
        }

        private byte[] bytes = null;
        private int length = 0;
    }
}