// -----------------------------------------------------------------
// File:    GameInstaller.cs
// Author:  mouguangyi
// Date:    2016.07.18
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(ServiceCenter))]
    [DisallowMultipleComponent]
    public class GameInstaller : ServiceInstaller<IGame>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IService Create()
        {
            return new Game();
        }
    }
}