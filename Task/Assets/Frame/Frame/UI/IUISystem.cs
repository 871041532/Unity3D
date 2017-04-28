// -----------------------------------------------------------------
// File:    IUISystem.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.UI
{
    /// <summary>
    /// @details UI系统。
    /// </summary>
    public interface IUISystem : IService
    {
        /// <summary>
        /// 创建一个窗体。
        /// </summary>
        /// <param name="path">窗体引用的prefab路径。</param>
        /// <param name="layerIndex">窗体的渲染层级。</param>
        /// <returns>返回窗体接口。</returns>
        IWindow CreateWindow(string path, int layerIndex);

        /// <summary>
        /// 异步创建一个窗体。
        /// </summary>
        /// <param name="path">窗体引用的prefab路径。</param>
        /// <param name="layerIndex">窗体的渲染层级。</param>
        /// <param name="callback">窗体创建完成的处理函数句柄。</param>
        void CreateWindowAsync(string path, int layerIndex, Action<IWindow> callback);

        /// <summary>
        /// 查询一个窗体。
        /// </summary>
        /// <param name="path">窗体引用的prefab路径。</param>
        /// <returns>返回第一个查到的窗体。</returns>
        IWindow FindWindow(string path);

        /// <summary>
        /// 查询一组窗体。
        /// </summary>
        /// <param name="path">窗体引用的prefab路径。</param>
        /// <returns>返回查到的窗体集。</returns>
        IWindow[] FindWindows(string path);

        /// <summary>
        /// 注册控件工厂。
        /// </summary>
        /// <param name="factory"></param>
        void RegisterFactory(IControlFactory factory);
    }
}
