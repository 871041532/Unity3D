// -----------------------------------------------------------------
// File:    IGiantGame.cs
// Author:  mouguangyi
// Date:    2016.07.18
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.GiantLightFramework
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGiantGame : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        void GotoScene(GiantLightScene scene);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void SetUserData<T>(string key, T data);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetUserData<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        bool IsConnected { get; }
    }
}