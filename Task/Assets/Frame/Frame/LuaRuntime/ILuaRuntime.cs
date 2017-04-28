// -----------------------------------------------------------------
// File:    ILuaRuntime.cs
// Author:  mouguangyi
// Date:    2016.07.22
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.LuaRuntime
{
    /// <summary>
    /// 第三方C Lua库的安装入口函数。LuaRuntime默认仅仅运行了纯Lua的运行环境和基础库，因此第三方的Lua库
    /// 需要通过InstallLibrary方法来装载。
    /// </summary>
    /// <param name="luaState">LuaRuntime当前的LuaState。</param>
    /// <returns></returns>
    public delegate int LuaLibraryFunction(IntPtr luaState);

    /// <summary>
    /// 
    /// </summary>
    public interface ILuaRuntime : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        object[] DoString(byte[] chunk);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        object[] DoFile(string fileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object[] Call(object function, params object[] args);

        /// <summary>
        /// 注册C#函数到Lua中。
        /// </summary>
        /// <param name="function"></param>
        /// <remarks>每一个暴露到Lua的函数必须要有LuaBridgeAttribute属性定义。</remarks>
        void RegLuaBridgeFunction(LuaBridgeFunction function);

        /// <summary>
        /// 在当前的LuaRuntime中安装其他的C Lua库。
        /// </summary>
        /// <param name="function">安装执行函数。</param>
        /// <returns>返回值为function的返回值。</returns>
        int InstallLibrary(LuaLibraryFunction function);
    }
}