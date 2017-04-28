// -----------------------------------------------------------------
// File:    NativeModule.cs
// Author:  mouguangyi
// Date:    2017.02.22
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.NativeChannel
{
    /// <summary>
    /// @details Native模块名。可分别设置相应平台的模块名称，为空则使用Default值。
    /// </summary>
    public struct NativeModule
    {
        /// <summary>
        /// 默认的模块名。
        /// </summary>
        public string Default;

        /// <summary>
        /// iOS平台模块名。
        /// </summary>
        public string IOS;

        /// <summary>
        /// Android平台的模块名。
        /// </summary>
        public string Android;
    }
}