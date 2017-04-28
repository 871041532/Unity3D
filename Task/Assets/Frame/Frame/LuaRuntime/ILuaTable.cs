// -----------------------------------------------------------------
// File:    ILuaTable.cs
// Author:  mouguangyi
// Date:    2016.08.11
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;

namespace GameBox.Service.LuaRuntime
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILuaTable
    {
        /// <summary>
        /// 将lua table转换成object[]。
        /// </summary>
        /// <returns></returns>
        object[] ToArray();

        /// <summary>
        /// 将lua table转换成Dictionary&lt;object, object&gt;()。
        /// </summary>
        /// <returns></returns>
        Dictionary<object, object> ToMap();
    }
}