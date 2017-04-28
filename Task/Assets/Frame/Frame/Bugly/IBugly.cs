// -----------------------------------------------------------------
// File:    IBugly.cs
// Author:  mouguangyi
// Date:    2017.02.04
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.Bugly
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    public delegate void LogCallbackDelegate(string condition, string stackTrace, UnityEngine.LogType type);

    /// <summary>
    /// 
    /// </summary>
    public interface IBugly : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void RegisterLogCallback(LogCallbackDelegate handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void UnregisterLogCallback(LogCallbackDelegate handler);
    }
}