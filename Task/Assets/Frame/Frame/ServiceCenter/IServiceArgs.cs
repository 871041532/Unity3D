// -----------------------------------------------------------------
// File:    IServiceArgs.cs
// Author:  mouguangyi
// Date:    2016.12.28
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details 服务初始化参数输入接口。
    /// </summary>
    public interface IServiceArgs
    {
        /// <summary>
        /// 设置指定值到指定名字的参数。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <param name="key">参数名。</param>
        /// <param name="value">参数值。</param>
        void Set<T>(string key, T value);
    }
}