
namespace GameFramework
{
    /// <summary>
    /// @details IService是每一个子Service需要实现的接口。
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Id是唯一标示一个Service的标识符，调用者通过这个Id可以查询到自己需要的Service实例，从而使用Service。Id的命名规则为com.giant.service.[Service Name]，例如com.giant.service.AService。
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Run是服务启动入口，ServiceCenter启动一个Service时，会调用该方法，从而给每一个Service初始化的机会。在这个函数中，Service需要执行其内部的初始化过程，当完成后必须要调用IServiceRunner的Ready方法来通知ServiceCenter，否则其他服务和外部程序是无法获取到这个服务的。
        /// </summary>
        /// <param name="runner">执行Service运行的沙箱运行器。</param>
        void Run(IServiceRunner runner);

        /// <summary>
        /// 如果Service需要在每一帧执行一些逻辑，则需要添加到该函数中。
        /// </summary>
        /// <param name="delta">帧间隔，单位为秒。</param>
        void Pulse(float delta);
    }
}
