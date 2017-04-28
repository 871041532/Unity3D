// -----------------------------------------------------------------
// File:    NativeModule.cs
// Author:  mouguangyi
// Date:    2017.02.22
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.NativeChannel
{
    /// <summary>
    /// @details Nativeģ�������ɷֱ�������Ӧƽ̨��ģ�����ƣ�Ϊ����ʹ��Defaultֵ��
    /// </summary>
    public struct NativeModule
    {
        /// <summary>
        /// Ĭ�ϵ�ģ������
        /// </summary>
        public string Default;

        /// <summary>
        /// iOSƽ̨ģ������
        /// </summary>
        public string IOS;

        /// <summary>
        /// Androidƽ̨��ģ������
        /// </summary>
        public string Android;
    }
}