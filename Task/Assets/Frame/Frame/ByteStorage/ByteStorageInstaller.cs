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
    /// @details ByteԤ����ռ����װ����
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�IByteStorage
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.bytestorage
    /// 
    /// @see IByteStorage
    ///  
    /// @section dependences ����
    /// �ޡ�
    /// 
    /// @section howtouse ʹ��
    /// ֱ����ק��ServicePlayer���ڵ�GameObject�ϼ��ɡ�
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_byte_storage_1_1_i_byte_storage.html")]
    [DisallowMultipleComponent]
    public sealed class ByteStorageInstaller : ServiceInstaller<IByteStorage>
    {
        /// <summary>
        /// ʹ��big-endian����little-endian��
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