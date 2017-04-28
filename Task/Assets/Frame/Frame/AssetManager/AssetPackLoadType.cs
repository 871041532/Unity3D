// -----------------------------------------------------------------
// File:    AssetPackLoadType.cs
// Author:  mouguangyi
// Date:    2016.07.08
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetManager
{
    // 资产包装载方式。
    enum AssetPackLoadType
    {
        // App内场景。
        INTERNALSCENE,

        // Resources目录下。
        RESOURCES,

        // StreamingAssets目录下。
        STREAMINGASSETS,

        // PersistentPathData目录下。
        PERSISTENTDATAPATH,

        // 远端服务器。
        REMOTE,
    }
}