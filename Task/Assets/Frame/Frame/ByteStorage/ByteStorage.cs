// -----------------------------------------------------------------
// File:    ByteStorage.cs
// Author:  mouguangyi
// Date:    2016.06.01
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;

namespace GameBox.Service.ByteStorage
{
    sealed class ByteStorage : IByteStorage, IServiceGraph
    {
        public const int MIN_BUFFER_SIZE = 64 * 1024;  // 64k
        public const int MIN_BYTEARRAY_SIZE = 32;      // 32b

        public static int GetProperSize(int size)
        {
            if (size <= MIN_BYTEARRAY_SIZE) {
                return MIN_BYTEARRAY_SIZE;
            }

            int total = MIN_BYTEARRAY_SIZE;
            while (total < size) {
                total <<= 1;
            }

            return total;
        }

        public ByteStorage()
        { }

        public string Id
        {
            get {
                return "com.giant.service.bytestorage";
            }
        }

        public void Run(IServiceRunner runner)
        {
            ByteArray.BigEndian = runner.GetArgs<bool>("BigEndian");

            this.blocks.Add(new ByteBlock(this, MIN_BUFFER_SIZE));
            runner.Ready(_Terminate);
        }

        public void Pulse(float delta)
        { }

        public IByteArray Alloc(int size)
        {
            ByteArray bytes = null;
            for (int i = 0; i < this.blocks.Count; ++i) {
                bytes = this.blocks[i].Capture(size);
                if (null != bytes) {
                    return bytes;
                }
            }

            Logger<IByteStorage>.L("Alloc a new block because there is no enough space for size [" + size + "].");
            var block = new ByteBlock(this, Math.Max(GetProperSize(size), MIN_BUFFER_SIZE));
            this.blocks.Add(block);

            return block.Capture(size);
        }

        private void _Terminate()
        {
            for (var i = 0; i < this.blocks.Count; ++i) {
                this.blocks[i].Dispose();
            }
            this.blocks = null;
        }

        private List<ByteBlock> blocks = new List<ByteBlock>();

        // ---------------------------------------------------------
        // Graph
        public void Draw()
        {
            for (var i = 0; i < this.blocks.Count; ++i) {
                this.blocks[i].StartGraph();
                this.blocks[i].DrawGraph(0, i * (this.blocks[0].GraphHeight + 2) + 1);
            }
        }

        public float Width
        {
            get {
                return this.blocks[0].GraphWidth + 2;
            }
        }

        public float Height
        {
            get {
                return this.blocks.Count * (this.blocks[0].GraphHeight + 2);
            }
        }
    }
}
