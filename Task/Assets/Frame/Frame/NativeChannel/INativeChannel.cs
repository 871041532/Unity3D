// -----------------------------------------------------------------
// File:    INativeChannel.cs
// Author:  mouguangyi
// Date:    2016.09.29
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.NativeChannel
{
    /// <summary>
    /// @details C#����Native��ı�׼��װ���񣬹淶����Э�飬�򻯽������̡�
    /// </summary>
    public interface INativeChannel : IService
    {
        /// <summary>
        /// Managed�㵥��һ���Ե���Native��ĺ�����
        /// </summary>
        /// <param name="module"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        void Call(NativeModule module, string method, params object[] args);

        /// <summary>
        /// C#���proxy������Native���Ŀ��ģ����������ӡ�
        /// </summary>
        /// <param name="targetModule"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        INativeProxy Connect<T>(NativeModule module, T proxy) where T : class;
    }
}