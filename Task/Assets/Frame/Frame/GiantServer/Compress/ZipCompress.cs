//// -----------------------------------------------------------------
//// File:    ZipCompress.cs
//// Author:  fuzhun
//// Date:    2016.08.04
//// Description:
////      
//// -----------------------------------------------------------------
//using System.IO;
//using System.Collections.Generic;
//using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
//using GameBox.Service.ByteStorage;

//namespace GameBox.Service.GiantFreeServer
//{
//    class DataBuffer
//    {
//        public DataBuffer(int size)
//        {
//            Buffer = new byte[size];
//        }

//        public byte[] Buffer;
//        public int Length = 0;
//    }

//    /// <summary>
//    /// Zip Compress and Decompress
//    /// </summary>
//    static class ZipCompress
//    {
//        private static DataBuffer sDataBuf = new DataBuffer(64 * 1024);

//        /// <summary>
//        /// Zip Compress
//        /// </summary>
//        /// <param name="srcBuf"></param>
//        /// <returns></returns>
//        public static byte[] Compress(byte[] srcBuf)
//        {
//            sDataBuf.Length = 0;
//            using (MemoryStream ms = new MemoryStream(sDataBuf.Buffer)) {
//                using (DeflaterOutputStream zipStream = new DeflaterOutputStream(ms)) {
//                    zipStream.Write(srcBuf, 0, srcBuf.Length);
//                    zipStream.Finish();

//                    byte[] ret = new byte[ms.Position];
//                    //for (int i = 0; i < ms.Position; ++i)
//                    //    ret[i] = ms.GetBuffer()[i];
//                    ms.Position = 0;
//                    ms.Read(ret, 0, ret.Length);

//                    return ret;
//                }
//            }
//        }

//        /// <summary>
//        /// Zip Decompress
//        /// </summary>
//        /// <param name="srcBuf"></param>
//        /// <returns></returns>
//        public static byte[] Decompress(byte[] srcBuf)
//        {
//            using (MemoryStream ms = new MemoryStream(srcBuf)) {
//                ms.Seek(0, System.IO.SeekOrigin.Begin);
//                using (InflaterInputStream zipStream = new InflaterInputStream(ms)) {
//                    List<byte> bytelist = new List<byte>();
//                    int b = zipStream.ReadByte();
//                    while (b > -1) {
//                        bytelist.Add((byte)b);
//                        b = zipStream.ReadByte();
//                    }
//                    return bytelist.ToArray();
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="output"></param>
//        /// <returns></returns>
//        public static bool Decompress(IByteArray input, IByteArray output)
//        {
//            using (MemoryStream ms = new MemoryStream(input.Buffer, input.Offset, input.Size)) {
//                ms.Seek(0, System.IO.SeekOrigin.Begin);
//                using (InflaterInputStream zipStream = new InflaterInputStream(ms)) {
//                    int b = zipStream.ReadByte();
//                    while (b > -1) {
//                        output.WriteByte((byte)b);
//                        b = zipStream.ReadByte();
//                    }

//                    output.Seek();
//                    return true;
//                }
//            }
//        }
//    }
//}