// -----------------------------------------------------------------
// File:    CmdSerializer.cs
// Author:  fuzhun
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using System;
//using System.Reflection;
using System.Runtime.InteropServices;
//using GameBox.Framework;
//using GameBox.Service.ByteStorage;

namespace GameBox.Service.GiantFreeServer
{
    /// <summary>
    /// command struct / byte[] transform
    /// </summary>
    static class CmdSerializer
    {
        public static byte[] StructToBytes(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        public static object BytesToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length) {
                return null;
            }

            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }

        //public static void BytesToStruct<T>(byte[] data, ref T ss)
        //{
        //    var storage = TaskCenter.GetService<IByteStorage>();
        //    var bytes = storage.Alloc(data.Length);
        //    bytes.WriteBytes(data, 0, data.Length);
        //    bytes.Seek();

        //    object obj = ss as object;  //boxing ss
        //    Type type = typeof(T);
        //    FieldInfo[] fieldInfos = type.GetFields();
        //    foreach(FieldInfo field in fieldInfos)
        //    {
        //        if (field.IsStatic)
        //            continue;

        //        if(field.FieldType == typeof(System.UInt16))
        //        {
        //            UInt16 value = bytes.ReadUInt16();
        //            field.SetValue(obj, value);
        //        }
        //        else if(field.FieldType == typeof(System.Int16))
        //        {
        //            Int16 value = bytes.ReadInt16();
        //            field.SetValue(obj, value);
        //        }
        //        else if (field.FieldType == typeof(System.Int32))
        //        {
        //            Int32 value = bytes.ReadInt32();
        //            field.SetValue(obj, value);
        //        }
        //        else if (field.FieldType == typeof(System.UInt32))
        //        {
        //            UInt32 value = bytes.ReadUInt32();
        //            field.SetValue(obj, value);
        //        }
        //        else if (field.FieldType == typeof(System.UInt64))
        //        {
        //            UInt64 value = bytes.ReadUInt64();
        //            field.SetValue(obj, value);
        //        }
        //        else if (field.FieldType == typeof(System.Byte))
        //        {
        //            byte value = bytes.ReadByte();
        //            field.SetValue(obj, value);
        //        }
        //        else if(field.FieldType == typeof(System.Char))
        //        {
        //            byte value = bytes.ReadByte();
        //            field.SetValue(obj, value);
        //        }
        //        else if(field.FieldType == typeof(System.String)
        //            || field.FieldType == typeof(System.Byte[]))
        //        {
        //            object[] attributes = field.GetCustomAttributes(true);
        //            foreach(object attr in attributes)
        //            {
        //                if(attr is MarshalAsAttribute)
        //                {
        //                    MarshalAsAttribute mar = attr as MarshalAsAttribute;
        //                    if(mar.Value == UnmanagedType.ByValTStr)
        //                    {
        //                        //string
        //                        byte[] tmp = bytes.ReadBytes(mar.SizeConst);
        //                        string str = GBKEncoding.GBKEncoder.Read(tmp);
        //                        field.SetValue(obj, str);
        //                    }
        //                    else if(mar.Value == UnmanagedType.ByValArray)
        //                    {
        //                        //byte[]
        //                        int size = 0;
        //                        var subType = mar.ArraySubType;
        //                        switch(subType)
        //                        {
        //                            case UnmanagedType.I1:
        //                            case UnmanagedType.U1:
        //                                size = mar.SizeConst * 1;
        //                                break;
        //                            case UnmanagedType.I2:
        //                            case UnmanagedType.U2:
        //                                size = mar.SizeConst * 2;
        //                                break;
        //                            case UnmanagedType.I4:
        //                            case UnmanagedType.U4:
        //                                size = mar.SizeConst * 4;
        //                                break;
        //                            case UnmanagedType.I8:
        //                            case UnmanagedType.U8:
        //                                size = mar.SizeConst * 8;
        //                                break;
        //                            default:
        //                                throw new Exception("Unsupported byte array unmanaged type");
        //                        }

        //                        byte[] tmp = bytes.ReadBytes(size);
        //                        field.SetValue(obj, tmp);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Unsupported filed type");
        //        }
        //    }

        //    ss = (T)obj;  //unboxing
        //}
    }

}

