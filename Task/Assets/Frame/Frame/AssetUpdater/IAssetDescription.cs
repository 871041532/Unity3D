// -----------------------------------------------------------------
// File:    IAssetDescription.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details 单个资产文件描述，接入端需要实现。
    /// </summary>
    public interface IAssetDescription
    {
        /// <summary>
        /// 文件路径。
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 文件大小。
        /// </summary>
        long Size { get; }

        /// <summary>
        /// 校验文件内容是否一致。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Equals(byte[] data);
    }
}