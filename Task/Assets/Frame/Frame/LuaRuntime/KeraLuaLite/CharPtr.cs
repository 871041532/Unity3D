using System;
using System.Text;

namespace KeraLuaLite
{
    struct CharPtr
    {
        public CharPtr(IntPtr ptrString)
            : this()
        {
            str = ptrString;
        }

        static public implicit operator CharPtr(IntPtr ptr)
        {
            return new CharPtr(ptr);
        }

        public static string StringFromNativeUtf8(IntPtr nativeUtf8, int len = 0)
        {
            if (len == 0) {
                while (System.Runtime.InteropServices.Marshal.ReadByte(nativeUtf8, len) != 0)
                    ++len;
            }

            if (len == 0)
                return string.Empty;

            byte[] buffer = new byte[len];
            System.Runtime.InteropServices.Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer, 0, len);
        }

        static private string PointerToString(IntPtr ptr)
        {
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
        }

        static private string PointerToString(IntPtr ptr, int length)
        {
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr, length);
        }

        static private byte[] PointerToBuffer(IntPtr ptr, int length)
        {
            byte[] buff = new byte[length];
            System.Runtime.InteropServices.Marshal.Copy(ptr, buff, 0, length);
            return buff;
        }

        public override string ToString()
        {
            if (str == IntPtr.Zero)
                return "";

            return PointerToString(str);
        }

        // Changed 2013-05-18 by Dirk Weltz
        // Changed because binary chunks, which are also transfered as strings
        // get corrupted by conversion to strings because of the encoding.
        public string ToString(int length)
        {
            if (str == IntPtr.Zero || 0 == length)
                return "";

            byte[] buff = PointerToBuffer(str, length);
            // Are the first four bytes "ESC Lua". If yes, than it is a binary chunk.
            // Didn't check on version of Lua, because it isn't relevant.
            if (length > 3 && buff[0] == 0x1B && buff[1] == 0x4C && buff[2] == 0x75 && buff[3] == 0x61) {
                // It is a binary chunk
                return _ToByteString(buff, length);
            } else {
                // If PointerToString failed, change to binary char array.
                // - mouguangyi 2016.08.15
                var s = PointerToString(str, length);
                return (string.IsNullOrEmpty(s) ? _ToByteString(buff, length) : s);
            }
        }

        // Convert native asni string to C# byte array directly.
        // - mouguangyi 2016.10.14
        public byte[] ToBytes(int length)
        {
            if (str == IntPtr.Zero || 0 == length)
                return (new byte[0]);

            return PointerToBuffer(str, length);
        }

        // No encoding conversion but set to char array directly.
        // - mouguangyi 2016.08.15
        private string _ToByteString(byte[] buff, int length)
        {
            var s = new StringBuilder(length);
            foreach (byte b in buff) {
                s.Append((char)b);
            }

            return s.ToString();
        }

        IntPtr str;
    }
}
