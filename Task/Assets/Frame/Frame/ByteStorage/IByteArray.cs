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
    /// @details Ѱַ��ʼλ�á�
    /// </summary>
    public enum SeekOrigin
    {
        /// <summary>
        /// ����ͷ��
        /// </summary>
        BEGIN = 0,

        /// <summary>
        /// ��ǰλ�á�
        /// </summary>
        CURRENT = 1,

        /// <summary>
        /// ����β��
        /// </summary>
        END = 2,
    }

    /// <summary>
    /// @details IByteArray��һ��byte���ݿ飬�Ǵ�IByteStorage�з�����������ʹ����һ��Ҫ����Release�����������¸�byte�����޷��ͷš���Ҫ����һ�����ݵ�IByteArray��Ҳ��Ҫ����Retain������������Ҫ��ʱ��Ҳ�м�ҪRelease��
    /// </summary>
    public interface IByteArray
    {
        /// <summary>
        /// IByteArray���ڵ�ԭʼbyte����
        /// <remarks>�����ԭʼbyte����Ҫ���ã����������byte������Խ��򸲸ǡ�</remarks>
        /// </summary>
        byte[] Buffer { get; }

        /// <summary>
        /// IByteArray��ԭʼbyte����ƫ��λ�á�
        /// </summary>
        int Offset { get; }

        /// <summary>
        /// ������byte���ݿ���ռ�ݵĴ�С��
        /// </summary>
        int Size { get; }

        /// <summary>
        /// ��ǰ�Ķ�дλ�á�
        /// </summary>
        int Position { get; }
        
        /// <summary>
        /// ������
        /// </summary>
        void Retain();

        /// <summary>
        /// �ͷš�
        /// </summary>
        void Release();

        /// <summary>
        /// ��ס�������ݣ����ֻ��״̬�����Ի��ն���ռ䡣
        /// </summary>
        void Submit();

        /// <summary>
        /// �޸ĵ�ǰ��дλ�á�
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="seekOrigin"></param>
        void Seek(int offset = 0, SeekOrigin seekOrigin = SeekOrigin.BEGIN);

        /// <summary>
        /// ǿ���������ݵĴ�С��
        /// </summary>
        /// <param name="size"></param>
        void SetSize(int size);

        /// <summary>
        /// ��һ���ֽڡ�
        /// </summary>
        /// <returns></returns>
        byte ReadByte();

        /// <summary>
        /// ��һ������ֵ��
        /// </summary>
        /// <returns></returns>
        bool ReadBool();

        /// <summary>
        /// ��һ��16λ�޷���������ֵ��
        /// </summary>
        /// <returns></returns>
        UInt16 ReadUInt16();

        /// <summary>
        /// ��һ��32λ�޷���������ֵ��
        /// </summary>
        /// <returns></returns>
        UInt32 ReadUInt32();

        /// <summary>
        /// ��һ��64λ�޷���������ֵ��
        /// </summary>
        /// <returns></returns>
        UInt64 ReadUInt64();

        /// <summary>
        /// ��һ��16λ�з���������ֵ��
        /// </summary>
        /// <returns></returns>
        Int16 ReadInt16();

        /// <summary>
        /// ��һ��32λ�з���������ֵ��
        /// </summary>
        /// <returns></returns>
        Int32 ReadInt32();

        /// <summary>
        /// ��һ��64λ�з���������ֵ��
        /// </summary>
        /// <returns></returns>
        Int64 ReadInt64();

        /// <summary>
        /// ��һ���з���������ֵ����ͬ��ReadInt32������
        /// </summary>
        /// <returns></returns>
        int ReadInt();

        /// <summary>
        /// ��һ����������
        /// </summary>
        /// <returns></returns>
        float ReadFloat();

        /// <summary>
        /// ��һ��UTF8��������
        /// </summary>
        /// <returns></returns>
        string ReadString();

        /// <summary>
        /// ��ȡָ�����ȵ�byte���顣
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        byte[] ReadBytes(int length = -1);

        /// <summary>
        /// ��ȡһ��ָ�����ȵ�IByteArray��
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        IByteArray ReadByteArray(int length = -1);

        /// <summary>
        /// дһ���ֽڡ�
        /// </summary>
        /// <param name="data"></param>
        void WriteByte(byte data);

        /// <summary>
        /// дһ������ֵ��
        /// </summary>
        /// <param name="data"></param>
        void WriteBool(bool data);

        /// <summary>
        /// дһ��16λ�޷���������ֵ��
        /// </summary>
        /// <param name="data"></param>
        void WriteUInt16(UInt16 data);

        /// <summary>
        /// дһ��32λ�޷���������ֵ��
        /// </summary>
        /// <param name="data"></param>
        void WriteUInt32(UInt32 data);

        /// <summary>
        /// дһ��64λ�޷���������ֵ��
        /// </summary>
        /// <param name="data"></param>
        void WriteUInt64(UInt64 data);

        /// <summary>
        /// дһ��16λ�з���������ֵ��
        /// </summary>
        /// <param name="data"></param>
        void WriteInt16(Int16 data);

        /// <summary>
        /// дһ��32λ�з���������ֵ��
        /// </summary>
        /// <param name="data"></param>
        void WriteInt32(Int32 data);

        /// <summary>
        /// дһ��64λ�з���������ֵ��
        /// </summary>
        /// <param name="data"></param>
        void WriteInt64(Int64 data);

        /// <summary>
        /// дһ���з���������ֵ����ͬ��WriteInt32������
        /// </summary>
        /// <param name="data"></param>
        void WriteInt(int data);

        /// <summary>
        /// дһ����������
        /// </summary>
        /// <param name="data"></param>
        void WriteFloat(float data);

        /// <summary>
        /// дһ��UTF8�ַ�����
        /// </summary>
        /// <param name="data"></param>
        void WriteString(string data);

        /// <summary>
        /// д��ָ��ƫ�ƺʹ�С��byte���顣
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        void WriteBytes(byte[] data, int offset, int size);

        /// <summary>
        /// д��һ��IByteArray��
        /// </summary>
        /// <param name="data"></param>
        void WriteByteArray(IByteArray data);
    }
}