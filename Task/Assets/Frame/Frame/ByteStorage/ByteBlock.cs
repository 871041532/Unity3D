// -----------------------------------------------------------------
// File:    ByteBlock.cs
// Author:  mouguangyi
// Date:    2016.06.01
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.ByteStorage
{
    class ByteBlock : C0
    {
        public ByteBlock(ByteStorage storage, int size)
        {
            this.storage = storage;
            this.buffer = new byte[size];
            this.header = new ByteArray(this, 0, size);
        }

        public override void Dispose()
        {
            this.buffer = null;
            this.header = null;

            base.Dispose();
        }

        public ByteStorage Storage
        {
            get {
                return this.storage;
            }
        }

        public byte[] Buffer
        {
            get {
                return this.buffer;
            }
        }

        public ByteArray Capture(int size)
        {
            size = ByteStorage.GetProperSize(size);

            lock (this) {
                var node = this.header;
                while (null != node) {
                    if (0 == node.RefCount && node.total >= size) {
                        if (node.total >= (size << 1)) {
                            node.SplitByteArray(size);
                        }

                        node.Retain();
                        return node;
                    }

                    node = node.next;
                }
            }

            return null;
        }

        private ByteStorage storage = null;
        private byte[] buffer = null;
        private ByteArray header = null;

        // ---------------------------------------------------------
        // Graph
        public void StartGraph()
        {
            if (!this.isGraphStarted) {
                this.graphWidth = this.graphHeight = Mathf.CeilToInt(Mathf.Sqrt(this.buffer.Length));

                int minByteArraySize = ByteStorage.MIN_BYTEARRAY_SIZE;
                while (1 != minByteArraySize) {
                    if (2 != minByteArraySize) {
                        this.graphWidth >>= 1;
                        minByteArraySize >>= 1;
                    }
                    this.graphHeight >>= 1;
                    minByteArraySize >>= 1;
                }

                this.graphTexture = new Texture2D(this.graphWidth, this.graphHeight, TextureFormat.ARGB32, false);
                this.isGraphStarted = true;
            }
        }

        public void DrawGraph(int xOffset, int yOffset)
        {
            var node = this.header;
            while (null != node) {
                var color = node.RefCount > 0 ? GraphStyle.RedColor : GraphStyle.GreenColor;
                int offset = node.offset / ByteStorage.MIN_BYTEARRAY_SIZE;
                int size = node.total / ByteStorage.MIN_BYTEARRAY_SIZE;
                for (var i = 0; i < size; ++i) {
                    int x = (offset + i) % this.graphWidth;
                    int y = (offset + i) / this.graphWidth;
                    this.graphTexture.SetPixel(x, y, color);
                }

                node = node.next;
            }
            this.graphTexture.Apply();
            GUI.DrawTexture(new Rect(xOffset, yOffset, this.graphWidth, this.graphHeight), this.graphTexture, ScaleMode.StretchToFill);
        }

        public int GraphWidth
        {
            get {
                return this.graphWidth;
            }
        }

        public int GraphHeight
        {
            get {
                return this.graphHeight;
            }
        }

        private int graphWidth = 0;
        private int graphHeight = 0;
        private Texture2D graphTexture = null;
        private bool isGraphStarted = false;
    }
}