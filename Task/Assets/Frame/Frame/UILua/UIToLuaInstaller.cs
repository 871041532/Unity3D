// -----------------------------------------------------------------
// File:    UIToLuaInstaller.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using GameBox.Framework;
using GameBox.Service.UI;
using GameBox.Service.LuaRuntime;

namespace GameBox.Service.UIToLua
{
    /// <summary>
    /// @details Lua中UI系统服务的安装器。
    /// 
    /// @li @c 对应的服务接口：IUIToLua
    /// @li @c 对应的服务ID：com.giant.service.uitolua
    /// 
    /// @see IUIToLua
    /// 
    /// @section dependencies 依赖
    /// @li @c LuaRuntimeInstaller
    /// @li @c UISystemInstaller
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    [HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_u_i_to_lua_1_1_i_u_i_to_lua.html")]
    [RequireComponent(typeof(LuaRuntimeInstaller))]
    [RequireComponent(typeof(UISystemInstaller))]
    public sealed class UIToLuaInstaller : ServiceInstaller<IUIToLua>
    {
        protected override IService Create()
        {
            return new UIToLua();
        }
    }
}
