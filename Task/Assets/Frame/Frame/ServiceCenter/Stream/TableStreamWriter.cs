// -----------------------------------------------------------------
// File:    TableStreamWriter.cs
// Author:  mouguangyi
// Date:    2016.12.05
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;
using System.IO;

namespace GameBox.Framework
{
    /// <summary>
    /// @details 映射表流对象同步写入器。
    /// </summary>
    public sealed class TableStreamWriter : TableStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public TableStreamWriter(Stream stream) : base(stream)
        {
            Seek(9);
            this.stringWriter = new StringStreamWriter(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            long offset = this.Position;
            foreach (var pair in this.keyMap) {
                this.stringWriter.WriteString(pair.Key);
                var offsets = pair.Value;
                this.stringWriter.WriteNumber(offsets.Count);
                while (offsets.Count > 0) {
                    this.stringWriter.WriteNumber(offsets.Dequeue());
                }
            }
            Seek(0);
            this.stringWriter.WriteNumber(offset);

            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteKeyByte(string key, byte value)
        {
            _SetOffset(GenerateKey(key), this.Position);
            this.stringWriter.WriteByte(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteKeyNumber(string key, long value)
        {
            _SetOffset(GenerateKey(key), this.Position);
            this.stringWriter.WriteNumber(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteKeyFloat(string key, double value)
        {
            _SetOffset(GenerateKey(key), this.Position);
            this.stringWriter.WriteFloat(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteKeyString(string key, string value)
        {
            _SetOffset(GenerateKey(key), this.Position);
            this.stringWriter.WriteString(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="separater"></param>
        public void WriteKeyIndexString(string key, string value, string separater)
        {
            _SetOffset(GenerateKey(key), this.Position);
            this.stringWriter.WriteIndexString(value, separater);
        }

        private void _SetOffset(string key, long offset)
        {
            Queue<long> offsets = null;
            if (!this.keyMap.TryGetValue(key, out offsets)) {
                this.keyMap[key] = offsets = new Queue<long>();
            }

            offsets.Enqueue(offset);
        }

        private StringStreamWriter stringWriter = null;
    }
}