// -----------------------------------------------------------------
// File:    RC5Encrypt.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantFreeServer
{
    /// <summary>
    /// RC5 Encrypt and Decrypt
    /// </summary>
    static class RC5Encrypt
    {
        /// <summary>
        /// RC5 Encrypt
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>加密后返回的数据(8的整数倍)</returns>
        public static byte[] Encrypt(byte[] data)
        {
            return RC5EncryptHandle.RC5Encrypt(data);
        }

        /// <summary>
        /// RC5 Decrypt
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <returns>解密后返回的数据(8的整数倍)</returns>
        public static byte[] Decrypt(byte[] data)
        {
            return RC5EncryptHandle.RC5Decrypt(data);
        }
    }
}