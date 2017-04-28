// -----------------------------------------------------------------
// File:    IRecycleFactory.cs
// Author:  mouguangyi
// Date:    2016.12.08
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.ObjectPool
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRecycleFactory
    {
        /// <summary>
        /// 同步创建IRecycleObject。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object CreateObject(string type);

        /// <summary>
        /// 异步创建IRecycleObject。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        void CreateObjectAsync(string type, Action<object> handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recycleObject"></param>
        void DestroyObject(object recycleObject);
    }
}