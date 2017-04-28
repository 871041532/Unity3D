// -----------------------------------------------------------------
// File:    ByteArray.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Text;

namespace GameBox.Service.ByteStorage
{
    class ByteArray : CRef<ByteArray>, IByteArray
    {
        public static bool BigEndian { get; set; }

        internal ByteArray(ByteBlock block, int offset, int total)
        {
            this.block = block;
            this.offset = offset;
            this.total = total;
            _Reset();
        }

        public override void Release()
        {
            lock (this.block) {
                base.Release();
            }
        }

        protected override void Dispose()
        {
            _Reset();

            if (null != this.next && 0 == this.next.RefCount) {
                this.total += this.next.total;
                this.next = this.next.next;
                if (null != this.next) {
                    this.next.prev = this;
                }
            }

            if (null != this.prev && 0 == this.prev.RefCount) {
                this.prev.total += this.total;
                this.prev.next = this.next;
                if (null != this.next) {
                    this.next.prev = this.prev;
                }
            }
        }

        public byte[] Buffer
        {
            get {
                return this.block.Buffer;
            }
        }

        public int Offset
        {
            get {
                return this.offset;
            }
        }

        public int Size
        {
            get {
                return this.size;
            }
        }

        public int Position
        {
            get {
                return this.position;
            }
        }

        public void Submit()
        {
            this.readOnly = true;
            this.position = 0;

            var size = ByteStorage.GetProperSize(this.size);
            if (this.total >= (size << 1)) {
                SplitByteArray(size);
            }
        }

        public void Seek(int offset = 0, SeekOrigin seekOrigin = SeekOrigin.BEGIN)
        {
            switch (seekOrigin) {
            case SeekOrigin.BEGIN:
                this.position = offset;
                break;
            case SeekOrigin.CURRENT:
                this.position += offset;
                break;
            case SeekOrigin.END:
                this.position = this.size + offset;
                break;
            }
        }

        public void SetSize(int size)
        {
            if (size > this.total) {
                throw new ArgumentOutOfRangeException();
            }

            this.size = size;
        }

        public byte ReadByte()
        {
            _VertifyInRange(1);

            byte value = this.block.Buffer[this.offset + this.position];
            this.position += 1;

            return value;
        }

        public bool ReadBool()
        {
            byte value = ReadByte();
            return (0 != value);
        }

        public UInt16 ReadUInt16()
        {
            _VertifyInRange(2);

            UInt16 value;
            if (BigEndian) {
                value = (UInt16)((this.block.Buffer[this.offset + this.position] << 8) +
                                 (this.block.Buffer[this.offset + this.position + 1]));
            } else {
                value = (UInt16)((this.block.Buffer[this.offset + this.position]) +
                                 (this.block.Buffer[this.offset + this.position + 1] << 8));
            }
            this.position += 2;

            return value;
        }

        public UInt32 ReadUInt32()
        {
            _VertifyInRange(4);

            UInt32 value;
            if (BigEndian) {
                value = (UInt32)((this.block.Buffer[this.offset + this.position] << 24) +
                                 (this.block.Buffer[this.offset + this.position + 1] << 16) +
                                 (this.block.Buffer[this.offset + this.position + 2] << 8) +
                                 (this.block.Buffer[this.offset + this.position + 3]));
            } else {
                value = (UInt32)((this.block.Buffer[this.offset + this.position]) +
                                 (this.block.Buffer[this.offset + this.position + 1] << 8) +
                                 (this.block.Buffer[this.offset + this.position + 2] << 16) +
                                 (this.block.Buffer[this.offset + this.position + 3] << 24));
            }
            this.position += 4;

            return value;
        }

        public UInt64 ReadUInt64()
        {
            _VertifyInRange(8);

            UInt64 value;
            if (BigEndian) {
                value = (UInt64)((this.block.Buffer[this.offset + this.position] << 56) +
                                 (this.block.Buffer[this.offset + this.position + 1] << 48) +
                                 (this.block.Buffer[this.offset + this.position + 2] << 40) +
                                 (this.block.Buffer[this.offset + this.position + 3] << 32) +
                                 (this.block.Buffer[this.offset + this.position + 4] << 24) +
                                 (this.block.Buffer[this.offset + this.position + 5] << 16) +
                                 (this.block.Buffer[this.offset + this.position + 6] << 8) +
                                 (this.block.Buffer[this.offset + this.position + 7]));
            } else {
                value = (UInt64)((this.block.Buffer[this.offset + this.position]) +
                                 (this.block.Buffer[this.offset + this.position + 1] << 8) +
                                 (this.block.Buffer[this.offset + this.position + 2] << 16) +
                                 (this.block.Buffer[this.offset + this.position + 3] << 24) +
                                 (this.block.Buffer[this.offset + this.position + 4] << 32) +
                                 (this.block.Buffer[this.offset + this.position + 5] << 40) +
                                 (this.block.Buffer[this.offset + this.position + 6] << 48) +
                                 (this.block.Buffer[this.offset + this.position + 7] << 56));
            }
            this.position += 8;

            return value;
        }

        public Int16 ReadInt16()
        {
            _VertifyInRange(2);

            Int16 value;
            if (BigEndian) {
                value = (Int16)((this.block.Buffer[this.offset + this.position] << 8) +
                                (this.block.Buffer[this.offset + this.position + 1]));
            } else {
                value = (Int16)((this.block.Buffer[this.offset + this.position]) +
                                (this.block.Buffer[this.offset + this.position + 1] << 8));
            }

            this.position += 2;

            return value;
        }

        public Int32 ReadInt32()
        {
            _VertifyInRange(4);

            Int32 value;
            if (BigEndian) {
                value = (Int32)((this.block.Buffer[this.offset + this.position] << 24) +
                                (this.block.Buffer[this.offset + this.position + 1] << 16) +
                                (this.block.Buffer[this.offset + this.position + 2] << 8) +
                                (this.block.Buffer[this.offset + this.position + 3]));
            } else {
                value = (Int32)((this.block.Buffer[this.offset + this.position]) +
                                (this.block.Buffer[this.offset + this.position + 1] << 8) +
                                (this.block.Buffer[this.offset + this.position + 2] << 16) +
                                (this.block.Buffer[this.offset + this.position + 3] << 24));
            }
            this.position += 4;

            return value;
        }

        public Int64 ReadInt64()
        {
            _VertifyInRange(8);

            Int64 value;
            if (BigEndian) {
                value = (Int64)((this.block.Buffer[this.offset + this.position] << 56) +
                                (this.block.Buffer[this.offset + this.position + 1] << 48) +
                                (this.block.Buffer[this.offset + this.position + 2] << 40) +
                                (this.block.Buffer[this.offset + this.position + 3] << 32) +
                                (this.block.Buffer[this.offset + this.position + 4] << 24) +
                                (this.block.Buffer[this.offset + this.position + 5] << 16) +
                                (this.block.Buffer[this.offset + this.position + 6] << 8) +
                                (this.block.Buffer[this.offset + this.position + 7]));
            } else {
                value = (Int64)((this.block.Buffer[this.offset + this.position]) +
                                (this.block.Buffer[this.offset + this.position + 1] << 8) +
                                (this.block.Buffer[this.offset + this.position + 2] << 16) +
                                (this.block.Buffer[this.offset + this.position + 3] << 24) +
                                (this.block.Buffer[this.offset + this.position + 4] << 32) +
                                (this.block.Buffer[this.offset + this.position + 5] << 40) +
                                (this.block.Buffer[this.offset + this.position + 6] << 48) +
                                (this.block.Buffer[this.offset + this.position + 7] << 56));
            }
            this.position += 8;

            return value;
        }

        public int ReadInt()
        {
            return ReadInt32();
        }

        public float ReadFloat()
        {
            _VertifyInRange(4);

            float value = BitConverter.ToSingle(this.block.Buffer, this.offset + this.position);
            this.position += 4;

            return value;
        }

        public string ReadString()
        {
            int length = ReadUInt16();
            _VertifyInRange(length);

            string value = Encoding.UTF8.GetString(this.block.Buffer, this.offset + this.position, length);
            this.position += length;

            return value;
        }

        public byte[] ReadBytes(int length = -1)
        {
            if (-1 == length) {
                length = this.size - this.position;
            }
            _VertifyInRange(length);

            var bytes = new byte[length];
            Array.Copy(this.block.Buffer, this.offset + this.position, bytes, 0, length);
            this.position += length;

            return bytes;
        }

        public IByteArray ReadByteArray(int length = -1)
        {
            if (-1 == length) {
                length = this.size - this.position;
            }
            _VertifyInRange(length);

            var bytes = this.block.Storage.Alloc(length);
            bytes.WriteBytes(this.block.Buffer, this.offset + this.position, length);
            this.position += length;

            bytes.Seek();
            return bytes;
        }

        public void WriteByte(byte data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(1);

            this.block.Buffer[this.offset + this.position] = data;
            this.position += 1;

            _UpdateSize(this.position);
        }

        public void WriteBool(bool data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(1);

            WriteByte(data ? (byte)1 : (byte)0);
        }

        public void WriteUInt16(UInt16 data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(2);

            if (BigEndian) {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 24 >> 24);
            } else {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 24 >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 16 >> 24);
            }

            this.position += 2;

            _UpdateSize(this.position);
        }

        public void WriteUInt32(UInt32 data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(4);

            if (BigEndian) {
                this.block.Buffer[this.offset + this.position] = (byte)(data >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 8 >> 24);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data << 24 >> 24);
            } else {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 24 >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 8 >> 24);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data >> 24);
            }

            this.position += 4;

            _UpdateSize(this.position);
        }

        public void WriteUInt64(UInt64 data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(8);

            if (BigEndian) {
                this.block.Buffer[this.offset + this.position] = (byte)(data >> 56);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 8 >> 56);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 16 >> 56);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data << 24 >> 56);
                this.block.Buffer[this.offset + this.position + 4] = (byte)(data << 32 >> 56);
                this.block.Buffer[this.offset + this.position + 5] = (byte)(data << 40 >> 56);
                this.block.Buffer[this.offset + this.position + 6] = (byte)(data << 48 >> 56);
                this.block.Buffer[this.offset + this.position + 7] = (byte)(data << 56 >> 56);
            } else {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 56 >> 56);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 48 >> 56);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 40 >> 56);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data << 32 >> 56);
                this.block.Buffer[this.offset + this.position + 4] = (byte)(data << 24 >> 56);
                this.block.Buffer[this.offset + this.position + 5] = (byte)(data << 16 >> 56);
                this.block.Buffer[this.offset + this.position + 6] = (byte)(data << 8 >> 56);
                this.block.Buffer[this.offset + this.position + 7] = (byte)(data >> 56);
            }
            this.position += 8;

            _UpdateSize(this.position);
        }

        public void WriteInt16(Int16 data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(2);

            if (BigEndian) {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 24 >> 24);
            } else {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 24 >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 16 >> 24);
            }
            this.position += 2;

            _UpdateSize(this.position);
        }

        public void WriteInt32(Int32 data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(4);

            if (BigEndian) {
                this.block.Buffer[this.offset + this.position] = (byte)(data >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 8 >> 24);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data << 24 >> 24);
            } else {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 24 >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 8 >> 24);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data >> 24);
            }
            this.position += 4;

            _UpdateSize(this.position);
        }

        public void WriteInt64(Int64 data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(8);

            if (BigEndian) {
                this.block.Buffer[this.offset + this.position] = (byte)(data >> 56);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 8 >> 56);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 16 >> 56);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data << 24 >> 56);
                this.block.Buffer[this.offset + this.position + 4] = (byte)(data << 32 >> 56);
                this.block.Buffer[this.offset + this.position + 5] = (byte)(data << 40 >> 56);
                this.block.Buffer[this.offset + this.position + 6] = (byte)(data << 48 >> 56);
                this.block.Buffer[this.offset + this.position + 7] = (byte)(data << 56 >> 56);
            } else {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 56 >> 56);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 48 >> 56);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 40 >> 56);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data << 32 >> 56);
                this.block.Buffer[this.offset + this.position + 4] = (byte)(data << 24 >> 56);
                this.block.Buffer[this.offset + this.position + 5] = (byte)(data << 16 >> 56);
                this.block.Buffer[this.offset + this.position + 6] = (byte)(data << 8 >> 56);
                this.block.Buffer[this.offset + this.position + 7] = (byte)(data >> 56);
            }
            this.position += 8;

            _UpdateSize(this.position);
        }

        public void WriteInt(int data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(4);

            if (BigEndian) {
                this.block.Buffer[this.offset + this.position] = (byte)(data >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 8 >> 24);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data << 24 >> 24);
            } else {
                this.block.Buffer[this.offset + this.position] = (byte)(data << 24 >> 24);
                this.block.Buffer[this.offset + this.position + 1] = (byte)(data << 16 >> 24);
                this.block.Buffer[this.offset + this.position + 2] = (byte)(data << 8 >> 24);
                this.block.Buffer[this.offset + this.position + 3] = (byte)(data >> 24);
            }
            this.position += 4;

            _UpdateSize(this.position);
        }

        public void WriteFloat(float data)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(4);

            byte[] bytes = BitConverter.GetBytes(data);
            if (!BigEndian) {
                Array.Reverse(bytes);
            }
            this.block.Buffer[this.offset + this.position] = bytes[0];
            this.block.Buffer[this.offset + this.position + 1] = bytes[1];
            this.block.Buffer[this.offset + this.position + 2] = bytes[2];
            this.block.Buffer[this.offset + this.position + 3] = bytes[3];
            this.position += 4;

            _UpdateSize(this.position);
        }

        public void WriteString(string data)
        {
            if (this.readOnly) {
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            _VerifyCapacity(bytes.Length + 2);

            WriteUInt16((UInt16)bytes.Length);
            for (int i = 0; i < bytes.Length; ++i) {
                this.block.Buffer[this.offset + this.position + 2 + i] = bytes[i];
            }
            this.position += (bytes.Length + 2);

            _UpdateSize(this.position);
        }

        public void WriteBytes(byte[] data, int offset, int size)
        {
            if (this.readOnly) {
                return;
            }

            _VerifyCapacity(size);

            Array.Copy(data, offset, this.block.Buffer, this.offset + this.position, size);
            this.position += size;

            _UpdateSize(this.position);
        }

        public void WriteByteArray(IByteArray data)
        {
            WriteBytes(data.Buffer, data.Offset + data.Position, data.Size - data.Position);
        }

        internal void SplitByteArray(int size)
        {
            var splitNode = new ByteArray(this.block, this.offset + size, this.total - size);
            this.total = size;

            if (null != this.next) {
                this.next.prev = splitNode;
                splitNode.next = this.next;
            }

            this.next = splitNode;
            splitNode.prev = this;
        }

        private void _UpdateSize(int size)
        {
            if (this.size < size) {
                this.size = size;
            }
        }

        private void _VerifyCapacity(int needSize)
        {
            if (this.total - this.position < needSize) {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void _VertifyInRange(int needSize)
        {
            if (this.size - this.position < needSize) {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void _Reset()
        {
            this.size = 0;
            this.position = 0;
            this.readOnly = false;
        }

        private ByteBlock block = null;
        private int size = 0;
        private int position = 0;
        private bool readOnly = false;

        internal int offset = 0;
        internal int total = 0;
        internal ByteArray prev = null;
        internal ByteArray next = null;
    }
}