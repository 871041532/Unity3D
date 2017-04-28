// -----------------------------------------------------------------
// File:    LuaProtocolBufferInstaller.cs
// Author:  gexiaoyi
// Date:    2016.08.11
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.LuaRuntime.ProtocolBuffer
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(LuaRuntimeInstaller))]
    [DisallowMultipleComponent]
    public sealed class LuaProtocolBufferInstaller : ServiceInstaller<ILuaProtocolBuffer>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IService Create()
        {
            return new LuaProtocolBuffer();
        }
    }
}