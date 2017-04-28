// -----------------------------------------------------------------
// File:    IRecycleManager.cs
// Author:  mouguangyi
// Date:    2017.03.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.ObjectPool
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRecycleManager : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poolType"></param>
        /// <param name="processer"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        IRecyclePool Create(string poolType, IRecycleProcesser processer, IRecycleFactory factory = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IRecyclePool Find(string poolType);
    }
}