// -----------------------------------------------------------------
// File:    GiantVoicePlayerToLua.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.GiantVoice;
using GameBox.Service.LuaRuntime;
using System;

namespace GameBox.Service.GiantVoiceLua
{
    class GiantVoicePlayerToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(GiantVoicePlayerToLua.RegisterPlayVoiceFunction);
            luaRuntime.RegLuaBridgeFunction(GiantVoicePlayerToLua.RegisterStopVoiceFunction);
            luaRuntime.RegLuaBridgeFunction(GiantVoicePlayerToLua.SetVoiceWords);
            luaRuntime.RegLuaBridgeFunction(GiantVoicePlayerToLua.SetRecordTime);
            luaRuntime.RegLuaBridgeFunction(GiantVoicePlayerToLua.SetVoiceBarLength);
        }

        [LuaBridge("giantVoicePlayer_registerPlay", 2, 0)]
        public static void RegisterPlayVoiceFunction(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var playVoiceComponent = parameters[0] as IGiantVoicePlayer;
            var voiceId = parameters[1].ToString();
            if (null != playVoiceComponent && null != voiceId)
            {
                var playSuccessfulHandler = parameters.Length > 2 ? parameters[2] : null;
                var playFailHandler = parameters.Length > 3 ? parameters[3] : null;
                playVoiceComponent.RegisterPlay(voiceId,
                    delegate () {
                        if(null != playSuccessfulHandler)
                        {
                            executer.Call(playSuccessfulHandler);
                        }
                    },
                    delegate (string error) 
                    {
                        if (null != playFailHandler)
                        {
                            executer.Call(playFailHandler, error);
                        }
                    });
            } 
        }

        [LuaBridge("giantVoicePlayer_registerStop", 1, 0)]
        public static void RegisterStopVoiceFunction(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var playVoiceComponent = parameters[0] as IGiantVoicePlayer;
            if (null != playVoiceComponent)
            {
                var stopVoiceCompletedHandler = parameters.Length > 1 ? parameters[1] : null;
                playVoiceComponent.RegisterStop(
                    delegate () {
                        if (null != stopVoiceCompletedHandler)
                        {
                            executer.Call(stopVoiceCompletedHandler);
                        }
                    });
            }
        }

        [LuaBridge("giantVoicePlayer_setVoiceWords", 2, 0)]
        public static void SetVoiceWords(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var playVoiceComponent = parameters[0] as IGiantVoicePlayer;
            var words = parameters[1].ToString();
            if (null != playVoiceComponent && null != words)
            {
                playVoiceComponent.SetVoiceWords(words);
            }
        }

        [LuaBridge("giantVoicePlayer_setRecordTime", 2, 0)]
        public static void SetRecordTime(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var playVoiceComponent = parameters[0] as IGiantVoicePlayer;
            var recordTime = Convert.ToSingle(parameters[1]);
            if (null != playVoiceComponent && recordTime > 0.1f)
            {
                var format = parameters.Length > 2 ? parameters[2].ToString() : "";
                playVoiceComponent.SetRecordTime(recordTime, format);
            }
        }

        [LuaBridge("giantVoicePlayer_setVoiceBarLength", 2, 0)]
        public static void SetVoiceBarLength(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var playVoiceComponent = parameters[0] as IGiantVoicePlayer;
            var length = Convert.ToSingle(parameters[1]);
            if (null != playVoiceComponent && 0 != length)
            {
                playVoiceComponent.SetVoiceBarLength(length);
            }
        }
    }
}