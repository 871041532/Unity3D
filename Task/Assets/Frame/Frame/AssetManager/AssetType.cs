// -----------------------------------------------------------------
// File:    AssetType.cs
// Author:  mouguangyi
// Date:    2016.06.15
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// @details 资产类型
    /// </summary>
    public enum AssetType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        /// 文本。
        /// </summary>
        TEXT = 0,

        /// <summary>
        /// 纹理。
        /// </summary>
        TEXTURE,

        /// <summary>
        /// 二进制。
        /// </summary>
        BYTES,

        /// <summary>
        /// 图集。
        /// </summary>
        SPRITEATLAS,

        /// <summary>
        /// 模板。
        /// </summary>
        PREFAB,

        /// <summary>
        ///  声音。
        /// </summary>
        AUDIOCLIP,

        /// <summary>
        /// 场景。
        /// </summary>
        SCENE,

        /// <summary>
        /// Animation Controller文件, .controller文件
        /// </summary>
        ANIMATIONCONTROLLER,
    }
}