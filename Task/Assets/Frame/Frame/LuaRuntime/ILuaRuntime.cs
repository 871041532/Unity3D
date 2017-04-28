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
    /// ������C Lua��İ�װ��ں�����LuaRuntimeĬ�Ͻ��������˴�Lua�����л����ͻ����⣬��˵�������Lua��
    /// ��Ҫͨ��InstallLibrary������װ�ء�
    /// </summary>
    /// <param name="luaState">LuaRuntime��ǰ��LuaState��</param>
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
        /// ע��C#������Lua�С�
        /// </summary>
        /// <param name="function"></param>
        /// <remarks>ÿһ����¶��Lua�ĺ�������Ҫ��LuaBridgeAttribute���Զ��塣</remarks>
        void RegLuaBridgeFunction(LuaBridgeFunction function);

        /// <summary>
        /// �ڵ�ǰ��LuaRuntime�а�װ������C Lua�⡣
        /// </summary>
        /// <param name="function">��װִ�к�����</param>
        /// <returns>����ֵΪfunction�ķ���ֵ��</returns>
        int InstallLibrary(LuaLibraryFunction function);
    }
}