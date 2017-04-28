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
        /// װ���������顣
        /// </summary>
        ALL = 0,

        /// <summary>
        /// װ����Χ���顣
        /// </summary>
        SURROUNDING = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILevelSystem : IService
    {
        /// <summary>
        /// ͬ��װ�������ؿ���
        /// </summary>
        /// <param name="character"></param>
        /// <param name="levelPath"></param>
        /// <param name="levelRoot"></param>
        void LoadLevel(ISceneCharacter character, string levelPath, GameObject levelRoot);

        /// <summary>
        /// ����װ�ز����첽װ�س�����
        /// </summary>
        /// <param name="character"></param>
        /// <param name="levelPath"></param>
        /// <param name="policy"></param>
        /// <param name="levelRoot"></param>
        /// <param name="handler"></param>
        void LoadLevelAsync(ISceneCharacter character, string levelPath, LevelLoadPolicy policy, GameObject levelRoot, Action handler);
    }
}