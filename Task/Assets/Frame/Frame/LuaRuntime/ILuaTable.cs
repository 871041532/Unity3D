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
        /// ��lua tableת����object[]��
        /// </summary>
        /// <returns></returns>
        object[] ToArray();

        /// <summary>
        /// ��lua tableת����Dictionary&lt;object, object&gt;()��
        /// </summary>
        /// <returns></returns>
        Dictionary<object, object> ToMap();
    }
}