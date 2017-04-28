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
    /// @details Lua����ʱ����װ����
    /// 
    /// @li @c ��Ӧ�ķ���ӿڣ�ILuaRuntime
    /// @li @c ��Ӧ�ķ���ID��com.giant.service.luaruntime
    /// 
    /// @see ILuaRuntime
    /// 
    /// @section dependencies ����
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