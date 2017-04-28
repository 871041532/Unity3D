// -----------------------------------------------------------------
// File:    DESEncrypt.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantFreeServer
{
    /// <summary>
    /// DES Encrypt and Decrypt
    /// </summary>
    static class DESEncrypt
    {
        private static DESEncryptHandle desEncryptHandle = new DESEncryptHandle();

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data">要加密的数据流</param>
        /// <param name="offset">开始加密的数据起始位置</param>
        /// <param name="len">要加密的数据长度，必须8的整数倍</param>
        public static bool Encrypt(byte[] data, int offset = 0, int len = -1)
        {
            if (-1 == len) {
                len = data.Length;
            }
            if (len % 8 != 0) {
                return false;
            }

            desEncryptHandle.encdec_des(data, offset, len, true);

            return true;
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="offset">解密数据起始位置</param>
        /// <param name="len">要解密的数据长度，必须为8的整数倍</param>
        /// <returns></returns>
        public static bool Decrypt(byte[] data, int offset = 0, int len = -1)
        {
            if (-1 == len) {
                len = data.Length;
            }
            if (len % 8 != 0) {
                return false;
            }

            desEncryptHandle.encdec_des(data, offset, len, false);
            return true;
        }

        public static void ResetDESKey(byte[] key, byte index)
        {
            desEncryptHandle.ResetKey(key, index);
        }

        public static void DES_encrypt1(uint[] data, bool enc)
        {
            desEncryptHandle.DES_encrypt1_wrapper(data, enc);
        }
    }
}
