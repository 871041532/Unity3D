// -----------------------------------------------------------------
// File:    IRecyclePool.cs
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
    public interface IRecyclePool
    {
        /// <summary>
        /// 预装载对象。
        /// @note 如果该IRecyclePool并没有绑定一个IRecycleFactory，那么该方法无效。
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="count"></param>
        void Preload(string objectType, int count);

        /// <summary>
        /// 预装载对象。
        /// @note 如果该IRecyclePool并没有绑定一个IRecycleFactory，那么该方法无效。
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        void PreloadAsync(string objectType, int count, Action callback);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <returns></returns>
        T Pick<T>(string objectType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="recycleObject"></param>
        void Drop(string objectType, object recycleObject);
    }
}