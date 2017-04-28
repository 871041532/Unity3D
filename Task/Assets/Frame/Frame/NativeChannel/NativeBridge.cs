// -----------------------------------------------------------------
// File:    NativeBridge.cs
// Author:  mouguangyi
// Date:    2016.09.29
// Description:
//      
// -----------------------------------------------------------------
#if UNITY_IOS
using AOT;
using System;
using System.Runtime.InteropServices;
#elif UNITY_ANDROID
using GameBox.Framework;
using UnityEngine;
#endif


namespace GameBox.Service.NativeChannel
{
    class NativeBridge
    {
        public static void Connect(string json)
        {
#if UNITY_IOS
            _IOSNativeChannel_Connect(json);
#elif UNITY_ANDROID
            _AndroidNativeChannel_Connect(json);
#endif
        }

        public static void Disconnect(string json)
        {
#if UNITY_IOS
            _IOSNativeChannel_Disconnect(json);
#elif UNITY_ANDROID
            _AndroidNativeChannel_Disconnect(json);
#endif
        }

        public static void Send(string json)
        {
#if UNITY_IOS
            _IOSNativeChannel_Send(json);
#elif UNITY_ANDROID
            _AndroidNativeChannel_Send(json);
#endif
        }

        public static string Receive()
        {
#if UNITY_IOS
            var str = _IOSNativeChannel_Receive();
            if (IntPtr.Zero == str) {
                return null;
            }

            return Marshal.PtrToStringAnsi(str);
#elif UNITY_ANDROID
            return _AndroidNativeChannel_Receive();
#else
            return null;
#endif
        }

#if UNITY_IOS
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IOSNativeChannel_Connect")]
        private static extern void _IOSNativeChannel_Connect(string json);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IOSNativeChannel_Disconnect")]
        private static extern void _IOSNativeChannel_Disconnect(string json);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IOSNativeChannel_Send")]
        private static extern void _IOSNativeChannel_Send(string json);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IOSNativeChannel_Receive")]
        private static extern IntPtr _IOSNativeChannel_Receive();
#elif UNITY_ANDROID
        private static readonly string ANDROIDNATIVECHANNEL = "com.giant.service.nativechannel.AndroidNativeChannel";

        private static void _AndroidNativeChannel_Connect(string json)
        {
            _AndroidClass.CallStatic("connect", json);
        }

        private static void _AndroidNativeChannel_Disconnect(string json)
        {
            _AndroidClass.CallStatic("disconnect", json);
        }

        private static void _AndroidNativeChannel_Send(string json)
        {
            _AndroidClass.CallStatic("send", json);
        }

        private static string _AndroidNativeChannel_Receive()
        {
            return _AndroidClass.CallStatic<string>("receive");
        }

        private static AndroidJavaClass _AndroidClass
        {
            get {
                if (null == androidClass) {
                    androidClass = new AndroidJavaClass(ANDROIDNATIVECHANNEL);
                }

                return androidClass;
            }
        }

        private static AndroidJavaClass androidClass = null;
#endif
    }
}