// -----------------------------------------------------------------
// File:    RecycleManagerInstaller.cs
// Author:  mouguangyi
// Date:    2016.12.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.ObjectPool
{
    /// <summary>
    /// @details ����ط���װ����
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�IObjectPool
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.objectpool
    /// 
    /// @see IObjectPool
    /// 
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_object_pool_1_1_i_object_pool.html")]
    [DisallowMultipleComponent]
    public sealed class RecycleManagerInstaller : ServiceInstaller<IRecycleManager>
    {
        protected override IService Create()
        {
            return new RecycleManager();
        }
    }
}