// -----------------------------------------------------------------
// File:    ISceneCharacter.cs
// Author:  mouguangyi
// Date:    2016.11.15
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISceneCharacter
    {
        /// <summary>
        /// 
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// 
        /// </summary>
        Quaternion Orientation { get; }
    }
}