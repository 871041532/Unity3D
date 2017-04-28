// -----------------------------------------------------------------
// File:    StringStreamAsyncReader.cs
// Author:  mouguangyi
// Date:    2016.12.08
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;

namespace GameBox.Framework
{
    /// <summary>
    /// @details 字符串流异步读取器。
    /// </summary>
    public sealed class StringStreamAsyncReader : StringStream
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="stream"></param>
        public StringStreamAsyncReader(Stream stream) : base(stream)
        {
            this.stringTable = new List<string>();
            this.asyncIO = new AsyncStreamIO();
        }

        /// <summary>
        /// 销毁函数。
        /// </summary>
        public override void Dispose()
        {
            this.asyncIO.Dispose();

            this.stringTable = null;
            this.asyncIO = null;

            base.Dispose();
        }

        /// <summary>
        /// 读一个字节。
        /// </summary>
        /// <param name="callback"></param>
        public void ReadByte(Action<byte> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadByteCommand(this, callback));
        }

        /// <summary>
        /// 读一个整数。
        /// </summary>
        /// <param name="callback"></param>
        public void ReadNumber(Action<long> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadNumberCommand(this, callback));
        }

        /// <summary>
        /// 读一个浮点数。
        /// </summary>
        /// <param name="callback"></param>
        public void ReadFloat(Action<double> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadFloatCommand(this, callback));
        }

        /// <summary>
        /// 读一个字符串。
        /// </summary>
        /// <param name="callback"></param>
        public void ReadString(Action<string> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadStringCommand(this, callback));
        }

        /// <summary>
        /// 读一个索引字符串。
        /// </summary>
        /// <param name="separater"></param>
        /// <param name="callback"></param>
        public void ReadIndexString(string separater, Action<string> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadIndexStringCommand(this, separater, callback));
        }

        /// <summary>
        /// 开始读取操作。
        /// </summary>
        /// <param name="readCountPerFrame">每一帧读多少个操作。</param>
        /// <param name="handler">读取完成的回调句柄。</param>
        public void StartAsync(int readCountPerFrame = 0, Action handler = null)
        {
            this.asyncIO.StartAsync(readCountPerFrame, handler);
        }

        private byte _ReadByte()
        {
            return (byte)this.stream.ReadByte();
        }

        private long _ReadNumber()
        {
            return this.stream.ReadNumber();
        }

        private double _ReadFloat()
        {
            return this.stream.ReadFloat();
        }

        private string _ReadString()
        {
            return this.stream.ReadString();
        }

        private string _ReadIndexString(string separater)
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
        private AsyncStreamIO asyncIO = null;

        // - Command
        private class ReadByteCommand : StreamIOCommand
        {
            public ReadByteCommand(StringStreamAsyncReader reader, Action<byte> callback)
            {
                this.reader = reader;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = (byte)this.reader._ReadByte();
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private StringStreamAsyncReader reader = null;
            private Action<byte> callback = null;
        }

        private class ReadNumberCommand : StreamIOCommand
        {
            public ReadNumberCommand(StringStreamAsyncReader reader, Action<long> callback)
            {
                this.reader = reader;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadNumber();
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private StringStreamAsyncReader reader = null;
            private Action<long> callback = null;
        }

        private class ReadFloatCommand : StreamIOCommand
        {
            public ReadFloatCommand(StringStreamAsyncReader reader, Action<double> callback)
            {
                this.reader = reader;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadFloat();
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private StringStreamAsyncReader reader = null;
            private Action<double> callback = null;
        }

        private class ReadStringCommand : StreamIOCommand
        {
            public ReadStringCommand(StringStreamAsyncReader reader, Action<string> callback)
            {
                this.reader = reader;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadString();
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private StringStreamAsyncReader reader = null;
            private Action<string> callback = null;
        }

        private class ReadIndexStringCommand : StreamIOCommand
        {
            public ReadIndexStringCommand(StringStreamAsyncReader reader, string separater, Action<string> callback)
            {
                this.reader = reader;
                this.separater = separater;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadIndexString(this.separater);
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private StringStreamAsyncReader reader = null;
            private string separater = null;
            private Action<string> callback = null;
        }
    }
}