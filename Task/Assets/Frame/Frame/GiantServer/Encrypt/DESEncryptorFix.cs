// -----------------------------------------------------------------
// File:    DESEncryptorFix.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.GiantFreeServer
{
    static class DESEncryptorFix
    {
        private static uint LenSended = 0;
        private static uint LenRecved = 0;
        private static uint EncryptMask = 0;

        public static void SetDESEcryptMask(uint mask)
        {
            EncryptMask = mask;
        }
        public static bool IsNeedEncpypt(bool isSend)
        {
            uint data = 0;
            if (isSend)
                data = LenSended;
            else
                data = LenRecved;

            uint i = 0x80000000;
            i = i >> (int)data;
            if ((EncryptMask & i) != 0)
                return true;

            return false;
        }


        public static void IncrementSendData()
        {
            if (++LenSended >= 32)
                LenSended = 0;
        }

        public static void IncrementRecvData()
        {
            if (++LenRecved >= 32)
                LenRecved = 0;
        }

        public static void ResetSendRecvCount()
        {
            LenSended = 0;
            LenRecved = 0;
        }

        private static UInt32 ReadUInt32(byte[] data, int index)
        {
            return (UInt32)(data[index] + (data[index + 1] << 8) + (data[index + 2] << 16) + (data[index + 3] << 24));
        }

        private static void WriteUInt32(byte[] data, int index, UInt32 b)
        {
            data[index] = (byte)(b << 24 >> 24);
            data[index + 1] = (byte)(b << 16 >> 24);
            data[index + 2] = (byte)(b << 8 >> 24);
            data[index + 3] = (byte)(b >> 24);
        }

        public static void EncDec_DES(byte[] data, int index, int len, bool isSend)
        {
            int offset = 0;
            while (offset <= len - 8) {
                bool bEncrypt = false;
                if (isSend) {
                    //send cmd ==> encode
                    bEncrypt = DESEncryptorFix.IsNeedEncpypt(true);
                    DESEncryptorFix.IncrementSendData();
                } else {
                    //recv cmd ==> decode
                    bEncrypt = DESEncryptorFix.IsNeedEncpypt(false);
                    DESEncryptorFix.IncrementRecvData();
                }

                if (bEncrypt) {
                    UInt32[] tmp = new UInt32[2];
                    tmp[0] = ReadUInt32(data, index + offset);
                    tmp[1] = ReadUInt32(data, index + offset + 4);

                    DESEncrypt.DES_encrypt1(tmp, isSend);

                    WriteUInt32(data, index + offset, tmp[0]);
                    WriteUInt32(data, index + offset + 4, tmp[1]);
                }

                offset += 8;
            }
        }

        public static void Reset()
        {
            DESEncryptorFix.LenSended = 0;
            DESEncryptorFix.LenRecved = 0;
            DESEncryptorFix.EncryptMask = 0;
        }
    }
}