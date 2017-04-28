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
    /// @details Nativeͨ����װ����
    /// </summary>
    public sealed class NativeChannelInstaller : ServiceInstaller<INativeChannel>
    {
        protected override IService Create()
        {
            return new NativeChannel();
        }
    }
}