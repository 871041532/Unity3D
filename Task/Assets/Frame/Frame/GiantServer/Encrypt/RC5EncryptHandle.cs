// -----------------------------------------------------------------
// File:    RC5EncryptHandle.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantFreeServer
{
    static class RC5EncryptHandle
    {
        private static uint[] RC5_KeyData = null;
        private static uint RC5_KeyData_Size = 26;
        private static uint RC5_32_MASK = uint.MaxValue;
        private static uint RC5_32_P = 0xb7e15163;
        private static uint RC5_32_Q = 0x9e3779b9;
        private static int rounds = 12;
        private static byte[] data = new byte[] { 0x3f, 0x79, 0xd5, 0xe2, 0x4a, 140, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 110, 0xcb };

        public static uint ROTATE_l32(uint x, int y)
        {
            return ((x << (y & 0xff)) | (x >> (0x20 - (y & 0xff))));
        }

        public static uint ROTATE_r32(uint x, uint y)
        {
            return ((x >> (int)(y & 0xff)) | (x << (int)(0x20 - (y & 0xff))));
        }

        public static uint rotL(uint x, uint y, int w)
        {
            return ((x << (int)(y & 0xff)) | (x >> (w - ((int)((ulong)(y & 0xff))))));
        }

        public static uint rotR(uint x, uint y, int w)
        {
            return ((x >> (int)(y & 0xff)) | (x << (w - ((int)((ulong)(y & 0xff))))));
        }

        public static void c2l(byte[] c, ref uint l, ref uint index)
        {
            l = c[index];
            index++;
            l |= (uint)(c[index] << 8);
            index++;
            l |= (uint)(c[index] << 16);
            index++;
            l |= (uint)(c[index] << 24);
            index++;
        }

        public static void c2ln(byte[] c, ref uint l1, ref uint l2, ref uint index, uint n)
        {
            l1 = 0;
            l2 = 0;
            switch (n) {
            case 1:
                index--;
                l1 = c[index];
                break;

            case 2:
                index--;
                l1 = (uint)(c[index] << 8);
                break;

            case 3:
                l1 = (uint)(c[index] << 0x10);
                break;

            case 4:
                index--;
                l1 = (uint)(c[index] << 0x18);
                break;

            case 5:
                index--;
                l2 = c[index];
                break;

            case 6:
                index--;
                l2 = (uint)(c[index] << 8);
                break;

            case 7:
                index--;
                l2 = (uint)(c[index] << 0x10);
                break;

            case 8:
                index--;
                l2 = (uint)(c[index] << 0x18);
                break;
            }
        }

        public static void generateSubKey()
        {
            if (RC5_KeyData == null) {
                uint num6;
                uint num7;
                int num14;
                RC5_KeyData = new uint[RC5_KeyData_Size];
                uint[] numArray = new uint[64];
                uint l = 0;
                uint num4 = 0;
                uint[] numArray2 = new uint[34];
                int index = 0;
                int num15 = 16;
                uint num16 = 0;
                int num8 = 0;
                while (num8 <= (num15 - 8)) {
                    c2l(data, ref l, ref num16);
                    numArray[index++] = l;
                    c2l(data, ref l, ref num16);
                    numArray[index++] = l;
                    num8 += 8;
                }
                if ((num15 - num8) != 0) {
                    num7 = (uint)(num15 & 7);
                    c2ln(data, ref l, ref num4, ref num16, num7);
                    numArray[index] = l;
                    numArray[index + 1] = num4;
                }
                int num11 = (num15 + 3) / 4;
                int num12 = (rounds + 1) * 2;
                numArray2[0] = RC5_32_P;
                num8 = 1;
                while (num8 < num12) {
                    numArray2[num8] = (numArray2[num8 - 1] + RC5_32_Q) & RC5_32_MASK;
                    num8++;
                }
                index = (num12 <= num11) ? num11 : num12;
                index *= 3;
                int num13 = num14 = 0;
                uint num5 = num6 = 0;
                for (num8 = 0; num8 < index; num8++) {
                    num7 = ((numArray2[num13] + num5) + num6) & RC5_32_MASK;
                    num5 = numArray2[num13] = ROTATE_l32(num7, 3);
                    int y = (int)(num5 + num6);
                    num7 = ((numArray[num14] + num5) + num6) & RC5_32_MASK;
                    num6 = numArray[num14] = ROTATE_l32(num7, y);
                    if (++num13 >= num12) {
                        num13 = 0;
                    }
                    if (++num14 >= num11) {
                        num14 = 0;
                    }
                }
                for (int i = 0; i < RC5_KeyData_Size; i++) {
                    RC5_KeyData[i] = numArray2[i];
                }
            }
        }

        private static byte[] rc5Encrypt(byte[] input)
        {
            int rc5Tmp = 0;
            uint rc5A = 0, rc5B = 0;
            uint[] rc5L = new uint[13], rc5R = new uint[13];
            byte[] rc5Output = new byte[16];

            while (rc5Tmp < 13) {
                rc5L[rc5Tmp] = rc5R[rc5Tmp] = 0;
                rc5Tmp++;
            }

            rc5Tmp = 0;
            while (rc5Tmp < 4) {
                rc5A += (uint)((input[rc5Tmp] & 0xff) << (8 * rc5Tmp));
                rc5B += (uint)((input[rc5Tmp + 4] & 0xff) << (8 * rc5Tmp));
                rc5Tmp++;
            }
            rc5L[0] = rc5A + RC5_KeyData[0];
            rc5R[0] = rc5B + RC5_KeyData[1];
            rc5Tmp = 1;
            while (rc5Tmp <= 12) {
                rc5L[rc5Tmp] = rotL(rc5L[rc5Tmp - 1] ^ rc5R[rc5Tmp - 1], rc5R[rc5Tmp - 1], 0x20) + RC5_KeyData[2 * rc5Tmp];
                rc5R[rc5Tmp] = rotL(rc5R[rc5Tmp - 1] ^ rc5L[rc5Tmp], rc5L[rc5Tmp], 0x20) + RC5_KeyData[(2 * rc5Tmp) + 1];
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4) {
                rc5Output[rc5Tmp] = (byte)((rc5L[12] >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4) {
                rc5Output[rc5Tmp + 4] = (byte)((rc5R[12] >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            return rc5Output;
        }

        public static byte[] rC5Decrypt(byte[] input)
        {
            int rc5Tmp = 0;
            uint[] rc5L = new uint[13], rc5R = new uint[13];
            byte[] rc5Output = new byte[16];

            while (rc5Tmp < 13) {
                rc5L[rc5Tmp] = rc5R[rc5Tmp] = 0;
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4) {
                rc5L[12] += (uint)((input[rc5Tmp] & 0xff) << (8 * rc5Tmp));
                rc5R[12] += (uint)((input[rc5Tmp + 4] & 0xff) << (8 * rc5Tmp));
                rc5Tmp++;
            }
            rc5Tmp = 12;
            while (rc5Tmp > 0) {
                rc5R[rc5Tmp - 1] = rotR(rc5R[rc5Tmp] - RC5_KeyData[(2 * rc5Tmp) + 1], rc5L[rc5Tmp], 32) ^ rc5L[rc5Tmp];
                rc5L[rc5Tmp - 1] = rotR(rc5L[rc5Tmp] - RC5_KeyData[2 * rc5Tmp], rc5R[rc5Tmp - 1], 32) ^ rc5R[rc5Tmp - 1];
                rc5Tmp--;
            }
            uint num = 0;
            uint num2 = 0;
            num = rc5L[0] - RC5_KeyData[0];
            num2 = rc5R[0] - RC5_KeyData[1];
            rc5Tmp = 0;
            while (rc5Tmp < 4) {
                rc5Output[rc5Tmp] = (byte)((num >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            rc5Tmp = 0;
            while (rc5Tmp < 4) {
                rc5Output[rc5Tmp + 4] = (byte)((num2 >> (8 * rc5Tmp)) & 0xff);
                rc5Tmp++;
            }
            return rc5Output;
        }

        public static byte[] RC5Encrypt(byte[] data)
        {
            int rc5m = 0;
            int length = data.Length;
            byte[] rc5Input = new byte[8];

            generateSubKey();
            rc5m = length % 8;
            if (rc5m != 0) {
                length += 8 - rc5m;
            }

            byte[] output = new byte[length];
            for (int i = 0; i < length; ++i) {
                if (i < data.Length) output[i] = data[i];
                else output[i] = 0;
            }

            int rc5Tmp = 0;
            int rc5Index = 0;
            while (rc5Index < length) {
                rc5Input[rc5Tmp] = output[rc5Index];
                rc5Tmp++;
                if (rc5Tmp == 8) {
                    byte[] ret = rc5Encrypt(rc5Input);
                    rc5Index -= 7;
                    rc5Tmp = 0;
                    while (rc5Tmp < 8) {
                        output[rc5Index] = ret[rc5Tmp];
                        rc5Index++;
                        rc5Tmp++;
                    }
                    rc5Index--;
                    rc5Tmp = 0;
                }
                rc5Index++;
            }

            return output;
        }

        public static byte[] RC5Decrypt(byte[] data)
        {
            int rc5m = 0;
            int length = data.Length;
            byte[] rc5Input = new byte[8];

            generateSubKey();
            rc5m = length % 8;
            if (rc5m != 0) {
                length += 8 - rc5m;
            }

            byte[] output = new byte[length];
            for (int i = 0; i < length; ++i) {
                if (i < data.Length) output[i] = data[i];
                else output[i] = 0;
            }

            int rc5Tmp = 0;
            int rc5Index = 0;
            while (rc5Index < length) {
                rc5Input[rc5Tmp] = output[rc5Index];
                rc5Tmp++;
                if (rc5Tmp == 8) {
                    byte[] ret = rC5Decrypt(rc5Input);
                    rc5Index -= 7;
                    rc5Tmp = 0;
                    while (rc5Tmp < 8) {
                        output[rc5Index] = ret[rc5Tmp];
                        rc5Index++;
                        rc5Tmp++;
                    }
                    rc5Index--;
                    rc5Tmp = 0;
                }
                rc5Index++;
            }

            return output;
        }
    }
}