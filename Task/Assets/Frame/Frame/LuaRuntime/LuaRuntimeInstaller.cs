// -----------------------------------------------------------------
// File:    LuaRuntimeInstaller.cs
// Author:  mouguangyi
// Date:    2016.07.22
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using UnityEngine;

namespace GameBox.Service.LuaRuntime
{
    /// <summary>
    /// @details Lua运行时服务安装器。
    /// 
    /// @li @c 对应的服务接口：ILuaRuntime
    /// @li @c 对应的服务ID：com.giant.service.luaruntime
    /// 
    /// @see ILuaRuntime
    /// 
    /// @section dependencies 依赖
    /// @li @c AssetManagerInstaller
    /// 
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_lua_runtime_1_1_i_lua_runtime.html")]
    [RequireComponent(typeof(AssetManagerInstaller))]
    public sealed class LuaRuntimeInstaller : ServiceInstaller<ILuaRuntime>
    {
        protected override IService Create()
        {
            return new LuaRuntime();
        }
    }
}