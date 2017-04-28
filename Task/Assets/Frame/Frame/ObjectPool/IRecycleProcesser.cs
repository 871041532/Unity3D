// -----------------------------------------------------------------
// File:    IRecycleProcesser.cs
// Author:  mouguangyi
// Date:    2017.03.14
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.ObjectPool
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRecycleProcesser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recycleObject"></param>
        void RecoverObject(object recycleObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recycleObject"></param>
        void ReclaimObject(object recycleObject);
    }
}