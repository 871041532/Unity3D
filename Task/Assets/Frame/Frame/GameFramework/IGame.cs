// -----------------------------------------------------------------
// File:    IGame.cs
// Author:  mouguangyi
// Date:    2016.07.18
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGame : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        void GotoScene(Scene scene);

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
    }
}