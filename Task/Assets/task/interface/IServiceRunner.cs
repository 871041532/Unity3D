
using System;

namespace GameFramework
{
    /// <summary>
    /// Service终止的代理。
    /// </summary>
    /// <param name="service">即将终止的Service。</param>
    public delegate void ServiceShutdown(IService service);

    /// <summary>
    /// @details IServiceRunner是Service的运行沙盒，用来隔离每一个Service，并控制Service的生命周期。
    /// </summary>
    public interface IServiceRunner
    {
        /// <summary>
        /// Service终止的事件，通过监听这个事件可以获取所有即将终止的Service的消息。
        /// </summary>
        event ServiceShutdown OnShutdown;

        /// <summary>
        /// 当一个Service准备完毕时，需要调用这个方法通知ServiceRunner已经可以提供服务，否则其中运行的Service永远处于等待状态。
        /// </summary>
        /// <param name="terminateMethod">Service终止方法，当Service触发Runner的Shutdown方法后，Runner会调用对应的TerminateMethod来让Service清理资源。</param>
        void Ready(Action terminateMethod);

        /// <summary>
        /// 当服务需要关闭时，需要调用运行Service的ServiceRunner来终止服务。
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 获取服务参数。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <param name="key">参数名。</param>
        /// <returns>返回对应的参数值。</returns>
        T GetArgs<T>(string key);
    }
}
