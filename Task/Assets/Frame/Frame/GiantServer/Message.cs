// -----------------------------------------------------------------
// File:    Message.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GameBox.Service.GiantFreeServer
{
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    struct stNullUserCmd
    {
        public byte byCmd;
        public byte byParam;
        public uint dwTimestamp;
    }

    // 征途系列游戏消息结构
    //  |  4B(header) |  6B(CmdParamTime) |    **B(body)     |
    //  |   不加密     |      加密压缩       |     加密压缩      |
    class Message : IMessage
    {
        public static int HeadSize = 4;
        public byte[] headerBuf = new byte[HeadSize];
        public stNullUserCmd bmdParamTime = new stNullUserCmd();
        private bool isCompress = false;

        public byte[] bodyData { get; set; }

        public Message(byte byCmd, byte byParam, uint dwTimestamp)
        {
            this.bmdParamTime.byCmd = byCmd;
            this.bmdParamTime.byParam = byParam;
            this.bmdParamTime.dwTimestamp = dwTimestamp;
        }

        private byte[] GetCommandBuffer()
        {
            byte[] ncm = CmdSerializer.StructToBytes(this.bmdParamTime);
            int totalLen = ncm.Length;
            if (this.bodyData != null)
                totalLen += this.bodyData.Length;

            byte[] con = new byte[totalLen];
            Array.Copy(ncm, con, ncm.Length);
            Array.Copy(this.bodyData, 0, con, ncm.Length, totalLen - ncm.Length);
            return con;
        }

        private void WriteCommandHeader(byte[] buffer, ushort bodyLength)
        {
            buffer[0] = (byte)(bodyLength << 24 >> 24);
            buffer[1] = (byte)(bodyLength << 16 >> 24);
            if (this.isCompress) {
                buffer[3] = (byte)64;
            }
        }

        private byte[] CompressCommandAndFillZero()
        {
            byte[] con = GetCommandBuffer();
            if (con == null)
                return null;

            byte[] com = con;
            //compress

            this.isCompress = false;
            if (con.Length > 32) {
                this.isCompress = true;
             //   com = ZipCompress.Compress(con);
            }


            //fill zero
            int allsize8 = ((com.Length + HeadSize + 7) / 8) * 8;
            byte[] buffer = new byte[allsize8];
            ushort bodylength = (ushort)(allsize8 - HeadSize);
            WriteCommandHeader(buffer, (ushort)bodylength);

            for (int i = HeadSize; i < buffer.Length; ++i) {
                if (i < com.Length + HeadSize)
                    buffer[i] = com[i - HeadSize];
                else
                    buffer[i] = 0;
            }

            return buffer;
        }

        public byte[] CompressAndRC5Encrypt()
        {
            byte[] buffer = CompressCommandAndFillZero();

            if (buffer == null)
                return null;

            //RC5Crypt
            return RC5Encrypt.Encrypt(buffer);
        }

        public byte[] CompressAndDESEncrypt()
        {
            byte[] buffer = CompressCommandAndFillZero();

            if (null == buffer)
                return null;

            //if (DESEncrypt.Encrypt(buffer))
            //    return buffer;
            DESEncryptorFix.EncDec_DES(buffer, 0, buffer.Length, true);

            return buffer;
        }

        public void ParseMessageInRC5Encrypt(byte[] data)
        {
            byte[] output = RC5Encrypt.Decrypt(data);
            int bodyLength = ((int)output[1] << 8) + output[0];
            this.isCompress = false;
            if (((int)output[3] & 0x40) != 0) {
                this.isCompress = true;
                byte[] unCompressData = null;
                using (MemoryStream ms = new MemoryStream()) {
                    ms.Write(output, HeadSize, bodyLength);
                    //unCompressData = ZipCompress.Decompress(ms.GetBuffer());
                }
            }

            byte[] nullCmdBytes = new byte[6];
            Array.Copy(output, 4, nullCmdBytes, 0, 6);
            object obj = CmdSerializer.BytesToStruct(nullCmdBytes, typeof(stNullUserCmd));
            if (obj is stNullUserCmd) {
                bmdParamTime = (stNullUserCmd)obj;
            }
        }

        public void SerizizeNullCmd(byte[] data)
        {
            byte[] nullCmdBytes = new byte[6];
            Array.Copy(data, HeadSize, nullCmdBytes, 0, 6);
            bmdParamTime = (stNullUserCmd)CmdSerializer.BytesToStruct(nullCmdBytes, typeof(stNullUserCmd));
        }

        public void SerilizeBodyCmd(byte[] data)
        {
            int realsize = data.Length - HeadSize - 6;
            byte[] bodyBytes = new byte[realsize];
            Array.Copy(data, HeadSize + 6, bodyBytes, 0, realsize);
            //CmdBody = (T)CmdSerializer.BytesToStruct(bodyBytes, typeof(T));
        }
    }
}


