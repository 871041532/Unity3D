// -----------------------------------------------------------------
// File:    ILuaExecuter.cs
// Author:  mouguangyi
// Date:    2016.07.28
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.LuaRuntime
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="executer"></param>
    public delegate void LuaBridgeFunction(ILuaExecuter executer);

    /// <summary>
    /// 
    /// </summary>
    public interface ILuaExecuter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object[] PopParameters();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        void PushResult(object result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object[] Call(object function, params object[] args);
    }
}