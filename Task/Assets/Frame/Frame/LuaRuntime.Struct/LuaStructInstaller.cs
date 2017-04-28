// -----------------------------------------------------------------
// File:    LuaStructInstaller.cs
// Author:  mouguangyi
// Date:    2016.08.04
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using UnityEngine;

namespace GameBox.Service.LuaRuntime.Struct
{
    /// <summary>
    /// @details Lua Struct服务安装器。
    /// 
    /// @li @c 对应的服务接口：ILuaStruct
    /// @li @c 对应的服务ID：com.giant.service.luaruntime.struct
    /// 
    /// @see ILuaStruct
    /// 
    /// @section dependencies 依赖
    /// @li @c LuaRuntimeInstaller
    /// 
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_lua_runtime_1_1_struct_1_1_i_lua_struct.html")]
    [RequireComponent(typeof(LuaRuntimeInstaller))]
    [DisallowMultipleComponent]
    public sealed class LuaStructInstaller : ServiceInstaller<ILuaStruct>
    {
        protected override IService Create()
        {
            return new LuaStruct();
        }
    }
}