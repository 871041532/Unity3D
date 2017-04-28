// -----------------------------------------------------------------
// File:    AssetLoader.cs
// Author:  mouguangyi
// Date:    2016.07.11
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.AssetManager
{
    abstract class AssetLoader
    {
        public AssetLoader(AssetManager manager, string path)
        {
            this.manager = manager;
            this.path = path;
        }

        public virtual void Dispose()
        {
            if (null != this.asset) {
                this.asset.Release();
                this.asset = null;
            }

            this.manager = null;
            this.handler = null;
        }

        protected void SetHandler(Action<object> handler)
        {
            this.handler = handler;
        }

        protected virtual void NotifyLoaded(object data)
        {
            new CompletedTask().Start().Continue(task =>
            {
                if (null != this.handler) {
                    this.handler(data);
                    this.handler = null;
                }

                return null;
            });
        }

        protected AssetManager manager = null;
        protected string path = null;
        protected Action<object> handler = null;
        protected Asset asset = null;
    }
}
