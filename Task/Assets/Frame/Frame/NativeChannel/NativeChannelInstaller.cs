// -----------------------------------------------------------------
// File:    NativeChannelInstaller.cs
// Author:  mouguangyi
// Date:    2016.09.29
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.NativeChannel
{
    /// <summary>
    /// @details Native通道安装器。
    /// </summary>
    public sealed class NativeChannelInstaller : ServiceInstaller<INativeChannel>
    {
        protected override IService Create()
        {
            return new NativeChannel();
        }
    }
}