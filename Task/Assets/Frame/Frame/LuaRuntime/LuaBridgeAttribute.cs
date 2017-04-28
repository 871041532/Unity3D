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
    /// C#��¶��Lua���ŽӺ������Զ��塣
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public sealed class LuaBridgeAttribute : Attribute
    {
        /// <summary>
        /// ��¶��Lua�еĺ�������
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Lua����ʱ��Ҫ����Ĳ���������
        /// </summary>
        public uint Input { get; private set; }

        /// <summary>
        /// �������صĽ��������
        /// </summary>
        public uint Output { get; private set; }

        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="name">��¶��Lua�еĺ�������</param>
        /// <param name="input">Lua����ʱ��Ҫ��������ٲ���������</param>
        /// <param name="output">�������صĽ��������</param>
        public LuaBridgeAttribute(string name, uint input, uint output)
        {
            this.Name = name;
            this.Input = input;
            this.Output = output;
        }
    }
}