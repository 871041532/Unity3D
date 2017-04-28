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
    /// @details Byte预分配空间服务接口。
    /// </summary>
    public interface IByteStorage : IService
    {
        /// <summary>
        /// 分配指定大小的IByteArray。
        /// </summary>
        /// <param name="size">IByteArray大小，以字节为单位。</param>
        /// <returns>分配成功的IByteArray接口。</returns>
        IByteArray Alloc(int size);
    }
}