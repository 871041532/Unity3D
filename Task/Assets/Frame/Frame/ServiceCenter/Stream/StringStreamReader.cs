// -----------------------------------------------------------------
// File:    StringStreamReader.cs
// Author:  mouguangyi
// Date:    2016.12.07
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;
using System.IO;

namespace GameBox.Framework
{
    /// <summary>
    /// @details �ַ�����ͬ����ȡ����
    /// </summary>
    public sealed class StringStreamReader : StringStream
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="stream"></param>
        public StringStreamReader(Stream stream) : base(stream)
        {
            this.stringTable = new List<string>();
        }

        /// <summary>
        /// ��һ���ֽڡ�
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return (byte)this.stream.ReadByte();
        }

        /// <summary>
        /// ��һ��������
        /// </summary>
        /// <returns></returns>
        public long ReadNumber()
        {
            return this.stream.ReadNumber();
        }

        /// <summary>
        /// ��һ����������
        /// </summary>
        /// <returns></returns>
        public double ReadFloat()
        {
            return this.stream.ReadFloat();
        }

        /// <summary>
        /// ��һ���ַ�����
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            return this.stream.ReadString();
        }

        /// <summary>
        /// ��һ�������ַ�����
        /// </summary>
        /// <param name="separater"></param>
        /// <returns></returns>
        public string ReadIndexString(string separater)
        {
            var length = this.stream.ReadNumber();
            var segments = new string[length];
            for (var i = 0; i < length; ++i) {
                var flag = this.stream.ReadByte();
                switch (flag) {
                case TINDEX:
                    var index = (int)this.stream.ReadNumber();
                    segments[i] = this.stringTable[index - 1];
                    break;
                case STRING:
                    var data = this.stream.ReadString();
                    this.stringTable.Add(data);
                    segments[i] = data;
                    break;
                }
            }

            return string.Join(separater, segments);
        }

        private List<string> stringTable = null;
    }
}