// -----------------------------------------------------------------
// File:    TableStreamAsyncReader.cs
// Author:  mouguangyi
// Date:    2016.12.07
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;

namespace GameBox.Framework
{
    /// <summary>
    /// @details 映射表流对象异步读取器。
    /// </summary>
    public sealed class TableStreamAsyncReader : TableStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="validate"></param>
        public TableStreamAsyncReader(Stream stream, bool validate = false) : base(stream)
        {
            this.validate = validate;
            this.stringReader = new StringStreamReader(stream);
            this.asyncIO = new AsyncStreamIO();

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
        public override void Dispose()
        {
            this.stringReader.Dispose();
            this.asyncIO.Dispose();

            this.stringReader = null;
            this.asyncIO = null;

            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public override void Blob(string key, Action action)
        {
            this.asyncIO.AddStreamIOCommand(new BlobBeginCommand(this, key));
            this.asyncIO.AddStreamIOCommand(new BlobActionCommand(action));
            this.asyncIO.AddStreamIOCommand(new BlobEndCommand(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        public void ReadKeyByte(string key, Action<byte> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadKeyByteCommand(this, key, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        public void ReadKeyNumber(string key, Action<long> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadKeyNumberCommand(this, key, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        public void ReadKeyFloat(string key, Action<double> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadKeyFloatCommand(this, key, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        public void ReadKeyString(string key, Action<string> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadKeyStringCommand(this, key, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="separater"></param>
        /// <param name="callback"></param>
        public void ReadKeyIndexString(string key, string separater, Action<string> callback)
        {
            this.asyncIO.AddStreamIOCommand(new ReadKeyIndexStringCommand(this, key, separater, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readCountPerFrame"></param>
        /// <param name="handler"></param>
        public void StartAsync(int readCountPerFrame = 0, Action handler = null)
        {
            this.asyncIO.StartAsync(readCountPerFrame, handler);
        }

        private byte _ReadKeyByte(string key)
        {
            return (_MoveStreamOffset(key) ? (byte)this.stringReader.ReadByte() : (byte)0);
        }

        private long _ReadKeyNumber(string key)
        {
            return (_MoveStreamOffset(key) ? this.stringReader.ReadNumber() : 0);
        }

        private double _ReadKeyFloat(string key)
        {
            return (_MoveStreamOffset(key) ? this.stringReader.ReadFloat() : 0);
        }

        private string _ReadKeyString(string key)
        {
            return (_MoveStreamOffset(key) ? this.stringReader.ReadString() : string.Empty);
        }

        private string _ReadKeyIndexString(string key, string separater)
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
        private AsyncStreamIO asyncIO = null;

        // -- Command
        private class BlobBeginCommand : StreamIOCommand
        {
            public BlobBeginCommand(TableStreamAsyncReader reader, string key)
            {
                this.reader = reader;
                this.key = key;
            }

            public override void Execute()
            {
                this.reader.keyStack.Push(this.key);
            }

            private TableStreamAsyncReader reader = null;
            private string key = null;
        }

        private class BlobActionCommand : StreamIOCommand
        {
            public BlobActionCommand(Action action)
            {
                this.action = action;
            }

            public override void Execute()
            {
                this.action();
            }

            private Action action = null;
        }

        private class BlobEndCommand : StreamIOCommand
        {
            public BlobEndCommand(TableStreamAsyncReader reader)
            {
                this.reader = reader;
            }

            public override void Execute()
            {
                this.reader.keyStack.Pop();
            }

            private TableStreamAsyncReader reader = null;
        }

        private class ReadKeyByteCommand : StreamIOCommand
        {
            public ReadKeyByteCommand(TableStreamAsyncReader reader, string key, Action<byte> callback)
            {
                this.reader = reader;
                this.key = key;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadKeyByte(this.key);
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private TableStreamAsyncReader reader = null;
            private string key = null;
            private Action<byte> callback = null;
        }

        private class ReadKeyNumberCommand : StreamIOCommand
        {
            public ReadKeyNumberCommand(TableStreamAsyncReader reader, string key, Action<long> callback)
            {
                this.reader = reader;
                this.key = key;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadKeyNumber(this.key);
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private TableStreamAsyncReader reader = null;
            private string key = null;
            private Action<long> callback = null;
        }

        private class ReadKeyFloatCommand : StreamIOCommand
        {
            public ReadKeyFloatCommand(TableStreamAsyncReader reader, string key, Action<double> callback)
            {
                this.reader = reader;
                this.key = key;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadKeyFloat(this.key);
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private TableStreamAsyncReader reader = null;
            private string key = null;
            private Action<double> callback = null;
        }

        private class ReadKeyStringCommand : StreamIOCommand
        {
            public ReadKeyStringCommand(TableStreamAsyncReader reader, string key, Action<string> callback)
            {
                this.reader = reader;
                this.key = key;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadKeyString(this.key);
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private TableStreamAsyncReader reader = null;
            private string key = null;
            private Action<string> callback = null;
        }

        private class ReadKeyIndexStringCommand : StreamIOCommand
        {
            public ReadKeyIndexStringCommand(TableStreamAsyncReader reader, string key, string separater, Action<string> callback)
            {
                this.reader = reader;
                this.key = key;
                this.separater = separater;
                this.callback = callback;
            }

            public override void Execute()
            {
                var value = this.reader._ReadKeyIndexString(this.key, this.separater);
                if (null != this.callback) {
                    this.callback(value);
                }
            }

            private TableStreamAsyncReader reader = null;
            private string key = null;
            private string separater = null;
            private Action<string> callback = null;
        }
    }
}