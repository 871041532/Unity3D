// -----------------------------------------------------------------
// File:    IServiceArgs.cs
// Author:  mouguangyi
// Date:    2016.12.28
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details �����ʼ����������ӿڡ�
    /// </summary>
    public interface IServiceArgs
    {
        /// <summary>
        /// ����ָ��ֵ��ָ�����ֵĲ�����
        /// </summary>
        /// <typeparam name="T">ֵ���͡�</typeparam>
        /// <param name="key">��������</param>
        /// <param name="value">����ֵ��</param>
        void Set<T>(string key, T value);
    }
}