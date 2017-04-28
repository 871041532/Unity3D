// -----------------------------------------------------------------
// File:    SceneGroupData.cs
// Author:  mouguangyi
// Date:    2016.12.13
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneGroupData : SceneObjectData
    {
        /// <summary>
        /// 
        /// </summary>
        public List<SceneObjectData> Objects { get; set; }
    }
}