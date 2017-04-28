// -----------------------------------------------------------------
// File:    ByteConverter.cs
// Author:  mouguangyi
// Date:    2016.06.02
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
//using ProtoBuf;
using System;
using System.IO;

namespace GameBox.Service.ByteStorage
{
    /// <summary>
    /// Byte��Protocol Buffers���໥ת�����ߡ�
    /// </summary>
    public class ByteConverter
    {
        /// <summary>
        /// Protocol Buffers�����ת��Ϊbyte���顣
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] ProtoBufToBytes<T>(T t)
        {
            try {
                using (var stream = new MemoryStream()) {
                    //Serializer.Serialize<T>(stream, t);
                    return stream.ToArray();
                }
            } catch (Exception e) {
                Logger<IByteStorage>.E("[ProtoBufToBytes]" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static object BytesToProtoBuf(Type type, byte[] bytes, int offset, int size)
        {
            try {
                using (var stream = new MemoryStream(bytes, offset, size)) {
                    return null;
                }
            } catch (Exception e) {
                Logger<IByteStorage>.E("[BytesToProtoBuf]" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// byte����ת��ΪProtocol Buffers�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static T BytesToProtoBuf<T>(byte[] bytes, int offset, int size)
        {
            try {
                using (var stream = new MemoryStream(bytes, offset, size)) {
                    return default(T);
                }
            } catch (Exception e) {
                Logger<IByteStorage>.E("[BytesToProtoBuf]" + e.Message);
                return default(T);
            }
        }

        /// <summary>
        /// Protocol Buffers�����ת��ΪIByteArray����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="output">Ԥ�����output�Ĵ�С</param>
        /// <param name="size"></param>
        /// <returns>true��ʾ�ɹ���false��ʾʧ��</returns>
        public static bool ProtoBufToByteArray<T>(T t, IByteArray output, int size)
        {
            try {
                using (var stream = new MemoryStream(output.Buffer, output.Offset, size)) {
                    //Serializer.Serialize(stream, t);
                    size = (int)stream.Position;    // !!! Be careful
                    output.SetSize((int)size);
                    output.Submit();
                    return true;
                }
            } catch (Exception e) {
                Logger<IByteStorage>.E("[ProtoBufToByteArray]" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static object ByteArrayToProtoBuf(Type type, IByteArray bytes, int offset, int size)
        {
            try {
                using (var stream = new MemoryStream(bytes.Buffer, bytes.Offset + offset, size)) {
                    return null;
                }
            } catch (Exception e) {
                Logger<IByteStorage>.E("[ByteArrayToProtoBuf]" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// IByteArray����ת��ΪProtocol Buffers�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static T ByteArrayToProtoBuf<T>(IByteArray bytes, int offset, int size)
        {
            try {
                using (var stream = new MemoryStream(bytes.Buffer, bytes.Offset + offset, size)) {
                    return default(T);
                }
            } catch (Exception e) {
                Logger<IByteStorage>.E("[ByteArrayToProtoBuf]" + e.Message);
                return default(T);
            }
        }
    }
}