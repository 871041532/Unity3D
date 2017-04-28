// -----------------------------------------------------------------
// File:    LuaBridgeAttribute.cs
// Author:  mouguangyi
// Date:    2016.07.28
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.LuaRuntime
{
    /// <summary>
    /// C#暴露到Lua的桥接函数属性定义。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public sealed class LuaBridgeAttribute : Attribute
    {
        /// <summary>
        /// 暴露到Lua中的函数名。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Lua调用时需要传入的参数个数。
        /// </summary>
        public uint Input { get; private set; }

        /// <summary>
        /// 函数返回的结果个数。
        /// </summary>
        public uint Output { get; private set; }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="name">暴露到Lua中的函数名。</param>
        /// <param name="input">Lua调用时需要传入的最少参数个数。</param>
        /// <param name="output">函数返回的结果个数。</param>
        public LuaBridgeAttribute(string name, uint input, uint output)
        {
            this.Name = name;
            this.Input = input;
            this.Output = output;
        }
    }
}