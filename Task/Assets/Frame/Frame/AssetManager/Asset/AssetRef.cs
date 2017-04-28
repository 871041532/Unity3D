// -----------------------------------------------------------------
// File:    AssetRef.cs
// Author:  mouguangyi
// Date:    2017.03.07
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetManager
{
    sealed class AssetRef : IAsset
    {
        public AssetRef(Asset asset)
        {
            this.asset = asset;
        }

        public void Dispose()
        {
            if (null != this.asset) {
                this.asset.Release();
                this.asset = null;
            }
        }

        public T Cast<T>()
        {
            if (null != this.asset.Data) {
                return (T)this.asset.Data;
            } else {
                return default(T);
            }
        }

        private Asset asset = null;
    }
}