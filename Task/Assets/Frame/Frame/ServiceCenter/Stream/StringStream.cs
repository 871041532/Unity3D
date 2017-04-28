// -----------------------------------------------------------------
// File:    StringStream.cs
// Author:  mouguangyi
// Date:    2016.12.07
// Description:
//      
// -----------------------------------------------------------------
using System.IO;

namespace GameBox.Framework
{
    public abstract class StringStream : C0
    {
        public StringStream(Stream stream)
        {
            this.stream = stream;
        }

        public override void Dispose()
        {
            this.stream = null;

            base.Dispose();
        }

        protected Stream stream = null;

        protected const byte TINDEX = 0x00;
        protected const byte STRING = 0x01;
    }
}