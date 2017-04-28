// -----------------------------------------------------------------
// File:    IAssetConfigure.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details 资产配置文件接口，接入端需要实现。
    /// </summary>
    public interface IAssetConfigure
    {
        /// <summary>
        /// 资产版本号，格式须符合GameBox.Framework.Version定义。
        /// 
        /// @see GameBox.Framework.Version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 资产描述列表。
        /// </summary>
        IAssetDescription[] Assets { get; }
    }
}