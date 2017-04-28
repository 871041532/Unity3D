// -----------------------------------------------------------------
// File:    StreamExtension.cs
// Author:  mouguangyi
// Date:    2016.12.05
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.IO;
using System.Text;

namespace GameBox.Framework
{
    /// <summary>
    /// @details Stream扩展类。
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// 写整数。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteNumber(this Stream stream, long value)
        {
            if (value < 256) {
                stream.WriteByte(INT8);
                stream.WriteByte((byte)value);
            } else if (value < short.MaxValue) {
                stream.WriteByte(INT16);
                stream.WriteByte((byte)(value << 16 >> 24));
                stream.WriteByte((byte)(value << 24 >> 24));
            } else if (value < short.MaxValue) {
                stream.WriteByte(INT32);
                stream.WriteByte((byte)(value >> 24));
                stream.WriteByte((byte)(value << 8 >> 24));
                stream.WriteByte((byte)(value << 16 >> 24));
                stream.WriteByte((byte)(value << 24 >> 24));
            } else {
                stream.WriteByte(INT64);
                stream.WriteByte((byte)(value >> 56));
                stream.WriteByte((byte)(value << 8 >> 56));
                stream.WriteByte((byte)(value << 16 >> 56));
                stream.WriteByte((byte)(value << 24 >> 56));
                stream.WriteByte((byte)(value << 32 >> 56));
                stream.WriteByte((byte)(value << 40 >> 56));
                stream.WriteByte((byte)(value << 48 >> 56));
                stream.WriteByte((byte)(value << 56 >> 56));
            }
        }

        /// <summary>
        /// 写浮点数。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteFloat(this Stream stream, double value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (value > float.MinValue && value < float.MaxValue) {
                stream.WriteByte(FLOAT32);
                for (var i = 4; i < 8; ++i) {
                    stream.WriteByte(bytes[i]);
                }
            } else {
                stream.WriteByte(FLOAT64);
                for (var i = 0; i < 8; ++i) {
                    stream.WriteByte(bytes[i]);
                }
            }
        }

        /// <summary>
        /// 写字符串。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteString(this Stream stream, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            stream.WriteNumber(bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 读整数。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static long ReadNumber(this Stream stream)
        {
            var flag = (byte)stream.ReadByte();
            switch (flag) {
            case INT8:
                return (long)((stream.ReadByte()));
            case INT16:
                return (long)((stream.ReadByte() << 8) +
                              (stream.ReadByte()));
            case INT32:
                return (long)((stream.ReadByte() << 24) +
                              (stream.ReadByte() << 16) +
                              (stream.ReadByte() << 8) +
                              (stream.ReadByte()));
            case INT64:
            default:
                return (long)((stream.ReadByte() << 56) +
                              (stream.ReadByte() << 48) +
                              (stream.ReadByte() << 40) +
                              (stream.ReadByte() << 32) +
                              (stream.ReadByte() << 24) +
                              (stream.ReadByte() << 16) +
                              (stream.ReadByte() << 8) +
                              (stream.ReadByte()));
            }
        }

        /// <summary>
        /// 读浮点数。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static double ReadFloat(this Stream stream)
        {
            var index = 0;
            var type = stream.ReadByte();
            if (FLOAT32 == type) {
                index = 4;
            }
            var bytes = new byte[8];
            for (var i = index; i < 8; ++i) {
                bytes[i] = (byte)stream.ReadByte();
            }

            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// 读字符串。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadString(this Stream stream)
        {
            var count = (int)stream.ReadNumber();
            var bytes = new byte[count];
            stream.Read(bytes, 0, count);
            return Encoding.UTF8.GetString(bytes);
        }

        private const byte FLOAT32 = 0xca;
        private const byte FLOAT64 = 0xcb;
        private const byte UINT8 = 0xcc;
        private const byte UINT16 = 0xcd;
        private const byte UINT32 = 0xce;
        private const byte UINT64 = 0xcf;
        private const byte INT8 = 0xd0;
        private const byte INT16 = 0xd1;
        private const byte INT32 = 0xd2;
        private const byte INT64 = 0xd3;
    }
}