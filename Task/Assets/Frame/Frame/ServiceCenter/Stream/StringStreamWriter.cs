// -----------------------------------------------------------------
// File:    StringStreamWriter.cs
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
    /// @details 字符串流同步写入器。
    /// </summary>
    public sealed class StringStreamWriter : StringStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public StringStreamWriter(Stream stream) : base(stream)
        {
            this.stringIndex = 0;
            this.stringMap = new Dictionary<string, int>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            this.stream.WriteByte(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteNumber(long value)
        {
            this.stream.WriteNumber(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteFloat(double value)
        {
            this.stream.WriteFloat(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            this.stream.WriteString(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separater"></param>
        public void WriteIndexString(string value, string separater)
        {
            var segments = value.Split(separater[0]);

            this.stream.WriteNumber(segments.Length);
            for (var i = 0; i < segments.Length; ++i) {
                var index = -1;
                if (this.stringMap.TryGetValue(segments[i], out index)) {
                    stream.WriteByte(TINDEX);
                    stream.WriteNumber(index);
                } else {
                    stream.WriteByte(STRING);
                    stream.WriteString(segments[i]);
                    this.stringMap.Add(segments[i], (++this.stringIndex));
                }
            }
        }

        private int stringIndex = 0;
        private Dictionary<string, int> stringMap = null;
    }
}