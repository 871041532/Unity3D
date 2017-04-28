// -----------------------------------------------------------------
// File:    AssetUpdateType.cs
// Author:  mouguangyi
// Date:    2016.08.29
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details 热更新状态类型。
    /// </summary>
    public enum AssetUpdateType
    {
        /// <summary>
        /// 无效。
        /// </summary>
        INVALID = -1,

        /// <summary>
        /// 已更新。
        /// </summary>
        UPDATED = 0,

        /// <summary>
        /// 需要热更新。
        /// </summary>
        HOTUPDATE = 1,

        /// <summary>
        /// 需要全版本强更新。
        /// </summary>
        FULLUPDATE = 2,
    }
}