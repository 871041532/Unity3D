// -----------------------------------------------------------------
// File:    GiantPackage.cs
// Author:  mouguangyi
// Date:    2016.06.16
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.ByteStorage;
using System;

namespace GameBox.Service.GiantLightServer
{
    abstract class GiantPackage : IDisposable
    {
        public GiantPackage(uint id)
        {
            this.id = id;
        }

        public virtual void Dispose()
        {
            if (null != this.bytes) {
                this.bytes.Release();
                this.bytes = null;
            }
        }

        public uint Id
        {
            get {
                return this.id;
            }
        }

        private uint id = 0;
        protected IByteArray bytes = null;
    }
}