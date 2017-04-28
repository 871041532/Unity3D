// -----------------------------------------------------------------
// File:    IByteStorage.cs
// Author:  mouguangyi
// Date:    2016.06.01
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.ByteStorage
{
    /// <summary>
    /// @details ByteԤ����ռ����ӿڡ�
    /// </summary>
    public interface IByteStorage : IService
    {
        /// <summary>
        /// ����ָ����С��IByteArray��
        /// </summary>
        /// <param name="size">IByteArray��С�����ֽ�Ϊ��λ��</param>
        /// <returns>����ɹ���IByteArray�ӿڡ�</returns>
        IByteArray Alloc(int size);
    }
}