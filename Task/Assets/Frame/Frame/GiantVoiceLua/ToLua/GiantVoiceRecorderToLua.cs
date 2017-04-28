// -----------------------------------------------------------------
// File:    GiantVoiceRecorderToLua.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.GiantVoice;
using GameBox.Service.LuaRuntime;

namespace GameBox.Service.GiantVoiceLua
{
    class GiantVoiceRecorderToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(GiantVoiceRecorderToLua.RegisterStopRecordFunction);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceRecorderToLua.RegisterCancelRecordFunction);
        }

        [LuaBridge("giantVoiceRecorder_registerStop", 2, 0)]
        public static void RegisterStopRecordFunction(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var recordVoiceButton = parameters[0] as IGiantVoiceRecorder;
            var recordSuccessfulHandler = parameters[1];
            if (null != recordVoiceButton && null != recordSuccessfulHandler)
            {
                var recordFailHandler = parameters.Length > 2 ? parameters[2] : null;
                recordVoiceButton.RegisterStop(
                    delegate (string voidId, float recordTime, string words) {
                        executer.Call(recordSuccessfulHandler, voidId, recordTime, words);
                    },
                    delegate (string error)
                    {
                        if (null != recordFailHandler)
                        {
                            executer.Call(recordFailHandler, error);
                        }
                    });
            }
        }

        [LuaBridge("giantVoiceRecorder_registerCancel", 1, 0)]
        public static void RegisterCancelRecordFunction(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var recordVoiceButton = parameters[0] as IGiantVoiceRecorder;
            if (null != recordVoiceButton)
            {
                var cancelSuccessfulHandler = parameters.Length > 1 ? parameters[1] : null;
                var cancelFailHandler = parameters.Length > 2 ? parameters[2] : null;
                recordVoiceButton.RegisterCancel(
                    delegate () {
                        if (null != cancelSuccessfulHandler)
                        {
                            executer.Call(cancelSuccessfulHandler);
                        }
                    },
                    delegate (string error)
                    {
                        if (null != cancelFailHandler)
                        {
                            executer.Call(cancelFailHandler, error);
                        }
                    });
            }
        }
    }
}