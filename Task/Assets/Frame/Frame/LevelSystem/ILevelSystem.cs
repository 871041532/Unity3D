// -----------------------------------------------------------------
// File:    ILevelSystem.cs
// Author:  mouguangyi
// Date:    2016.11.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// 
    /// </summary>
    public enum LevelLoadPolicy
    {
        /// <summary>
        /// 装载所有区块。
        /// </summary>
        ALL = 0,

        /// <summary>
        /// 装载周围区块。
        /// </summary>
        SURROUNDING = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILevelSystem : IService
    {
        /// <summary>
        /// 同步装载整个关卡。
        /// </summary>
        /// <param name="character"></param>
        /// <param name="levelPath"></param>
        /// <param name="levelRoot"></param>
        void LoadLevel(ISceneCharacter character, string levelPath, GameObject levelRoot);

        /// <summary>
        /// 根据装载策略异步装载场景。
        /// </summary>
        /// <param name="character"></param>
        /// <param name="levelPath"></param>
        /// <param name="policy"></param>
        /// <param name="levelRoot"></param>
        /// <param name="handler"></param>
        void LoadLevelAsync(ISceneCharacter character, string levelPath, LevelLoadPolicy policy, GameObject levelRoot, Action handler);
    }
}