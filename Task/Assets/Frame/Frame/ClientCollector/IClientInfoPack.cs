// -----------------------------------------------------------------
// File:    IClientInfoPack.cs
// Author:  mouguangyi
// Date:    2016.05.16
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.ClientCollector
{
    /// <summary>
    /// 客户端的基础信息包。
    /// </summary>
    public interface IClientInfoPack
    {
        /// <summary>
        /// 设备模式，
        /// </summary>
        string DeviceMode { get; }

        /// <summary>
        /// 设备名称。
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// 设备唯一标识符。
        /// </summary>
        string DeviceIdentifier { get; }

        /// <summary>
        /// 图形设备标识符。
        /// </summary>
        int GraphicsDeviceId { get; }

        /// <summary>
        /// 图形设备名称。
        /// </summary>
        string GraphicsDeviceName { get; }

        /// <summary>
        /// 图形设备类型。
        /// </summary>
        int GraphicsDeviceType { get; }

        /// <summary>
        /// 图形设备代理商。
        /// </summary>
        string GraphicsDeviceVendor { get; }

        /// <summary>
        /// 图形设备版本。
        /// </summary>
        string GraphicsDeviceVersion { get; }

        /// <summary>
        /// 图形设备内存大小。
        /// </summary>
        int GraphicsMemorySize { get; }

        /// <summary>
        /// 操作系统。
        /// </summary>
        string OperatingSystem { get; }

        /// <summary>
        /// 设备处理器个数。
        /// </summary>
        int ProcessorCount { get; }

        /// <summary>
        /// 设备处理器频率。
        /// </summary>
        int ProcessorFrequency { get; }

        /// <summary>
        /// 设备处理器类型。
        /// </summary>
        string ProcessorType { get; }

        /// <summary>
        /// 是否支持陀螺仪。
        /// </summary>
        bool DoesSupportGyroscope { get; }

        /// <summary>
        /// 是否支持定位服务。
        /// </summary>
        bool DoesSupportLocationService { get; }

        /// <summary>
        /// 设备内存大小。
        /// </summary>
        int SystemMemorySize { get; }

        /// <summary>
        /// 网络类型。
        /// </summary>
        string NetworkReachability { get; }

        /// <summary>
        /// 屏幕分辨率宽度。
        /// </summary>
        int ResolutionWidth { get; }

        /// <summary>
        /// 屏幕分辨率高度。
        /// </summary>
        int ResolutionHeight { get; }

        /// <summary>
        /// 屏幕刷新率。
        /// </summary>
        int ResolutionRefreshRate { get; }

        /// <summary>
        /// 国家。
        /// </summary>
        string Nation { get; }

        /// <summary>
        /// 运营商名称。
        /// </summary>
        string Carrier { get; }
    }
}