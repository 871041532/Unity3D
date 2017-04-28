// -----------------------------------------------------------------
// File:    ILuaString.cs
// Author:  mouguangyi
// Date:    2016.10.14
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.LuaRuntime
{
    /// <summary>
    /// Lua string (native asni char*)
    /// </summary>
    public interface ILuaString
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}