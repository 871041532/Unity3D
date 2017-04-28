// -----------------------------------------------------------------
// File:    ByteStorageInstaller.cs
// Author:  mouguangyi
// Date:    2016.06.01
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.ByteStorage
{
    /// <summary>
    /// @details Byte预分配空间服务安装器。
    /// 
    /// @li @c 对应的服务接口：IByteStorage
    /// @li @c 对应的服务ID：com.giant.service.bytestorage
    /// 
    /// @see IByteStorage
    ///  
    /// @section dependences 依赖
    /// 无。
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_byte_storage_1_1_i_byte_storage.html")]
    [DisallowMultipleComponent]
    public sealed class ByteStorageInstaller : ServiceInstaller<IByteStorage>
    {
        /// <summary>
        /// 使用big-endian还是little-endian。
        /// </summary>
        [Tooltip("IByteArray network sequence, default is BigEndian.")]
        public bool BigEndian = true;

        protected override IService Create()
        {
            return new ByteStorage();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("BigEndian", this.BigEndian);
        }
    }
}