// -----------------------------------------------------------------
// File:    LevelSystemInstaller.cs
// Author:  mouguangyi
// Date:    2016.11.15
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using GameBox.Service.ObjectPool;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// @details 关卡服务。
    /// 
    /// @li @c 对应的服务接口：ILevelSystem
    /// @li @c 对应的服务ID：com.giant.service.levelsystem
    /// 
    /// @see ILevelSystem
    /// 
    /// @section dependences 依赖
    /// @li @c AssetManagerInstaller
    /// @li @c ObjectPoolInstaller
    /// 
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_level_system_1_1_i_level_system.html")]
    [RequireComponent(typeof(AssetManagerInstaller))]
    [RequireComponent(typeof(RecycleManagerInstaller))]
    public sealed class LevelSystemInstaller : ServiceInstaller<ILevelSystem>
    {
        /// <summary>
        /// 是否校验数据。
        /// </summary>
        [Tooltip("Disable will have better performance.")]
        public bool ValidateData = false;

        protected override IService Create()
        {
            return new LevelSystem();
        }

        protected override void Arguments(IServiceArgs args)
        {
            args.Set("ValidateData", this.ValidateData);
        }
    }
}