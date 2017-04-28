// -----------------------------------------------------------------
// File:    IByteArray.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.ByteStorage
{
    /// <summary>
    /// @details 寻址起始位置。
    /// </summary>
    public enum SeekOrigin
    {
        /// <summary>
        /// 数据头。
        /// </summary>
        BEGIN = 0,

        /// <summary>
        /// 当前位置。
        /// </summary>
        CURRENT = 1,

        /// <summary>
        /// 数据尾。
        /// </summary>
        END = 2,
    }

    /// <summary>
    /// @details IByteArray是一个byte数据块，是从IByteStorage中分配出来，因此使用完一定要调用Release方法，否则导致该byte数据无法释放。若要保留一个传递的IByteArray，也需要调用Retain方法，但不需要的时候也切记要Release。
    /// </summary>
    public interface IByteArray
    {
        /// <summary>
        /// IByteArray所在的原始byte流。
        /// <remarks>这个是原始byte流，要慎用，否则会引起byte流数据越界或覆盖。</remarks>
        /// </summary>
        byte[] Buffer { get; }

        /// <summary>
        /// IByteArray在原始byte流的偏移位置。
        /// </summary>
        int Offset { get; }

        /// <summary>
        /// 内容在byte数据块中占据的大小。
        /// </summary>
        int Size { get; }

        /// <summary>
        /// 当前的读写位置。
        /// </summary>
        int Position { get; }
        
        /// <summary>
        /// 保留。
        /// </summary>
        void Retain();

        /// <summary>
        /// 释放。
        /// </summary>
        void Release();

        /// <summary>
        /// 锁住数据内容，变成只读状态并尝试回收多余空间。
        /// </summary>
        void Submit();

        /// <summary>
        /// 修改当前读写位置。
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="seekOrigin"></param>
        void Seek(int offset = 0, SeekOrigin seekOrigin = SeekOrigin.BEGIN);

        /// <summary>
        /// 强制设置内容的大小。
        /// </summary>
        /// <param name="size"></param>
        void SetSize(int size);

        /// <summary>
        /// 读一个字节。
        /// </summary>
        /// <returns></returns>
        byte ReadByte();

        /// <summary>
        /// 读一个布尔值。
        /// </summary>
        /// <returns></returns>
        bool ReadBool();

        /// <summary>
        /// 读一个16位无符号整型数值。
        /// </summary>
        /// <returns></returns>
        UInt16 ReadUInt16();

        /// <summary>
        /// 读一个32位无符号整型数值。
        /// </summary>
        /// <returns></returns>
        UInt32 ReadUInt32();

        /// <summary>
        /// 读一个64位无符号整型数值。
        /// </summary>
        /// <returns></returns>
        UInt64 ReadUInt64();

        /// <summary>
        /// 读一个16位有符号整型数值。
        /// </summary>
        /// <returns></returns>
        Int16 ReadInt16();

        /// <summary>
        /// 读一个32位有符号整型数值。
        /// </summary>
        /// <returns></returns>
        Int32 ReadInt32();

        /// <summary>
        /// 读一个64位有符号整型数值。
        /// </summary>
        /// <returns></returns>
        Int64 ReadInt64();

        /// <summary>
        /// 读一个有符号整型数值。等同于ReadInt32方法。
        /// </summary>
        /// <returns></returns>
        int ReadInt();

        /// <summary>
        /// 读一个浮点数。
        /// </summary>
        /// <returns></returns>
        float ReadFloat();

        /// <summary>
        /// 读一个UTF8字数串。
        /// </summary>
        /// <returns></returns>
        string ReadString();

        /// <summary>
        /// 读取指定长度的byte数组。
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        byte[] ReadBytes(int length = -1);

        /// <summary>
        /// 读取一个指定长度的IByteArray。
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        IByteArray ReadByteArray(int length = -1);

        /// <summary>
        /// 写一个字节。
        /// </summary>
        /// <param name="data"></param>
        void WriteByte(byte data);

        /// <summary>
        /// 写一个布尔值。
        /// </summary>
        /// <param name="data"></param>
        void WriteBool(bool data);

        /// <summary>
        /// 写一个16位无符号整型数值。
        /// </summary>
        /// <param name="data"></param>
        void WriteUInt16(UInt16 data);

        /// <summary>
        /// 写一个32位无符号整型数值。
        /// </summary>
        /// <param name="data"></param>
        void WriteUInt32(UInt32 data);

        /// <summary>
        /// 写一个64位无符号整型数值。
        /// </summary>
        /// <param name="data"></param>
        void WriteUInt64(UInt64 data);

        /// <summary>
        /// 写一个16位有符号整型数值。
        /// </summary>
        /// <param name="data"></param>
        void WriteInt16(Int16 data);

        /// <summary>
        /// 写一个32位有符号整型数值。
        /// </summary>
        /// <param name="data"></param>
        void WriteInt32(Int32 data);

        /// <summary>
        /// 写一个64位有符号整型数值。
        /// </summary>
        /// <param name="data"></param>
        void WriteInt64(Int64 data);

        /// <summary>
        /// 写一个有符号整型数值。等同于WriteInt32方法。
        /// </summary>
        /// <param name="data"></param>
        void WriteInt(int data);

        /// <summary>
        /// 写一个浮点数。
        /// </summary>
        /// <param name="data"></param>
        void WriteFloat(float data);

        /// <summary>
        /// 写一个UTF8字符串。
        /// </summary>
        /// <param name="data"></param>
        void WriteString(string data);

        /// <summary>
        /// 写入指定偏移和大小的byte数组。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        void WriteBytes(byte[] data, int offset, int size);

        /// <summary>
        /// 写入一个IByteArray。
        /// </summary>
        /// <param name="data"></param>
        void WriteByteArray(IByteArray data);
    }
}