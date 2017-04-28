// -----------------------------------------------------------------
// File:    GiantVoiceToLuaInstaller.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using GameBox.Framework;
using GameBox.Service.LuaRuntime;
using GameBox.Service.GameAnalytics;

namespace GameBox.Service.GameAnalyticsLua
{
    /// <summary>
    /// @details Lua中GameAnalytics系统服务的安装器。
    /// 
    /// @li @c 对应的服务接口：IGameAnalyticsToLua
    /// @li @c 对应的服务ID：com.giant.service.gameanalyticstolua
    /// 
    /// @see IGameAnalyticsToLua
    /// 
    /// @section dependencies 依赖
    /// @li @c LuaRuntimeInstaller
    /// @li @c GameAnalyticsInstaller
    /// 
    /// @section howtouse 使用
    /// 直接拖拽到ServicePlayer所在的GameObject上即可。
    /// </summary>
    //[HelpURL(DOCROOT + "interface_game_box_1_1_service_1_1_u_i_to_lua_1_1_i_u_i_to_lua.html")]
    [RequireComponent(typeof(LuaRuntimeInstaller))]
    [RequireComponent(typeof(GameAnalyticsInstaller))]
    public sealed class GameAnalyticsToLuaInstaller : ServiceInstaller<IGameAnalyticsToLua>
    {
        protected override IService Create()
        {
            return new GameAnalyticsToLua();
        }
    }
}

