// -----------------------------------------------------------------
// File:    ServiceInstaller.cs
// Author:  mouguangyi
// Date:    2016.05.11
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details 每一个Service都需要实现一个对应的安装器，用来实例化具体的Service。
    /// </summary>
    /// <typeparam name="T">Service对应的接口。</typeparam>
    public abstract class ServiceInstaller<T> : ServiceBaseInstaller where T : IService
    {
        /// <summary>
        /// 是否开启当前Service的日志。默认为true。
        /// </summary>
        public bool EnableLog = true;

        /// <summary>
        /// Service实例化方法，每一个Service模块必须要实现该方法。
        /// </summary>
        /// <returns>Service实例。</returns>
        protected abstract IService Create();

        /// <summary>
        /// Service初始化参数设置入口，安装器可以将需要的初始化参数通过IServiceArgs接口传入Service。
        /// </summary>
        /// <param name="args">IServiceArgs接口。</param>
        protected virtual void Arguments(IServiceArgs args)
        { }

        internal override void _Install(ServiceCenter center)
        {
            var service = Create();
            if (null != service) {
                var args = new ServiceArgs();
                Arguments(args);
                center._AddRunner(new ServiceRunner<T>(center, args, service, this.EnableLog));
            }
        }

        protected const string DOCROOT = "http://192.168.150.238/GameBox/help/";
    }
}
