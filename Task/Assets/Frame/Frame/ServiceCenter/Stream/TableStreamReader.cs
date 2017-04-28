// -----------------------------------------------------------------
// File:    TableStreamReader.cs
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
    /// @details 映射表流对象同步读取器。
    /// </summary>
    public sealed class TableStreamReader : TableStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="validate"></param>
        public TableStreamReader(Stream stream, bool validate = false) : base(stream)
        {
            this.validate = validate;
            this.stringReader = new StringStreamReader(stream);

            if (this.validate) {
                var offset = this.stringReader.ReadNumber();
                Seek(offset);
                {
                    while (this.Position < this.Length) {
                        var key = this.stringReader.ReadString();
                        var count = this.stringReader.ReadNumber();
                        var offsets = new Queue<long>();
                        for (var i = 0; i < count; ++i) {
                            offsets.Enqueue(this.stringReader.ReadNumber());
                        }
                        this.keyMap.Add(key, offsets);
                    }
                }
            }
            Seek(9);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte ReadKeyByte(string key)
        {
            return (_MoveStreamOffset(key) ? (byte)this.stringReader.ReadByte() : (byte)0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ReadKeyNumber(string key)
        {
            return (_MoveStreamOffset(key) ? this.stringReader.ReadNumber() : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double ReadKeyFloat(string key)
        {
            return (_MoveStreamOffset(key) ? this.stringReader.ReadFloat() : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadKeyString(string key)
        {
            return (_MoveStreamOffset(key) ? this.stringReader.ReadString() : string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="separater"></param>
        /// <returns></returns>
        public string ReadKeyIndexString(string key, string separater)
        {
            return (_MoveStreamOffset(key) ? this.stringReader.ReadIndexString(separater) : string.Empty);
        }

        private long _GetOffset(string key)
        {
            Queue<long> offsets = null;
            if (this.keyMap.TryGetValue(GenerateKey(key), out offsets)) {
                return offsets.Dequeue();
            }

            return -1;
        }

        private bool _MoveStreamOffset(string key)
        {
            long offset = 0;
            if (this.validate) {
                offset = _GetOffset(key);
                if (offset >= 0) {
                    Seek(offset);
                }
            }

            return (offset >= 0);
        }

        private bool validate = false;
        private StringStreamReader stringReader = null;
    }
}