// -----------------------------------------------------------------
// File:    GiantVoiceToLua.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.GiantVoice;
using GameBox.Service.LuaRuntime;

namespace GameBox.Service.GiantVoiceLua
{
    class GiantVoiceToLua : IGiantVoiceToLua
    {
        public static IGiantVoice giantVoice;
        public string Id
        {
            get
            {
                return "com.giant.service.giantvoicetolua";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServicesTask(new string[]{
                "com.giant.service.giantvoice",
                "com.giant.service.luaruntime",
            }).Start().Continue(task =>
            {
                var services = task.Result as IService[];
                giantVoice = services[0] as IGiantVoice;
                var luaRuntime = services[1] as ILuaRuntime;

                GiantVoiceToLua.RegLuaBridgeFunction(luaRuntime);
                GiantVoicePlayerToLua.RegLuaBridgeFunction(luaRuntime);
                GiantVoiceRecorderToLua.RegLuaBridgeFunction(luaRuntime);

                runner.Ready(_Terminate);
                return null;
            });
        }

        public void Pulse(float delata)
        { }

        private void _Terminate()
        { }

        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            /*luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.StartRecordVoice);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.StopRecordVoice);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.CancelRecordVoice);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.PlayVoice);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.PauseVoice);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.ResumeVoice);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.StopVoice);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.SetPlayVolume);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.GetCurrentPlayPosition);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.IsRecording);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.IsVoicePlaying);
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.IsVoicePause);*/
            luaRuntime.RegLuaBridgeFunction(GiantVoiceToLua.Clear);
        }


        /*[LuaBridge("giantVoice_startRecordVoice", 0, 0)]
        public static void StartRecordVoice(ILuaExecuter executer)
        {
            giantVoice.StartRecordVoice();
        }

        [LuaBridge("giantVoice_stopRecordVoice", 1, 0)]
        public static void StopRecordVoice(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var recordSuccessfulHandler = parameters[0];
            if(null != recordSuccessfulHandler)
            {
                var recordFailHandler = parameters.Length > 1 ? parameters[1] : null;
                giantVoice.StopRecordVoice(
                    delegate (string voiceId, float recordTime, string words) {
                        executer.Call(recordSuccessfulHandler, voiceId, recordTime, words);
                    }, 
                    delegate (string error) {
                        if(null != recordFailHandler)
                        {
                            executer.Call(recordFailHandler, error);
                        }
                    });
            }
        }

        [LuaBridge("giantVoice_cancelRecordVoice", 0, 0)]
        public static void CancelRecordVoice(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var cancelSuccessfulHandler = parameters.Length > 0 ? parameters[0] : null;
            var cancelFailHandler = parameters.Length > 1 ? parameters[1] : null;
            giantVoice.CancelRecordVoice(
                delegate () {
                    if (null != cancelSuccessfulHandler)
                    {
                        executer.Call(cancelSuccessfulHandler);
                    }
                },
                delegate (string error) {
                    if (null != cancelFailHandler)
                    {
                        executer.Call(cancelFailHandler, error);
                    }
                });
        }

        [LuaBridge("giantVoice_playVoice", 1, 0)]
        public static void PlayVoice(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var voiceId = parameters[0].ToString();
            if (null != voiceId)
            {
                var playSuccessfulHandler = parameters.Length > 1 ? parameters[1] : null;
                var playFailHandler = parameters.Length > 2 ? parameters[2] : null;
                giantVoice.PlayVoice(voiceId,
                    delegate () {
                        if(null != playSuccessfulHandler)
                        {
                            executer.Call(playSuccessfulHandler);
                        }
                    },
                    delegate (string error) {
                        if (null != playFailHandler)
                        {
                            executer.Call(playFailHandler, error);
                        }
                    });
            }
        }

        [LuaBridge("giantVoice_pauseVoice", 0, 0)]
        public static void PauseVoice(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var pauseVoiceCompletedHandler = parameters.Length > 0 ? parameters[0] : null;
            giantVoice.PauseVoice(
                delegate () {
                    if (null != pauseVoiceCompletedHandler)
                    {
                        executer.Call(pauseVoiceCompletedHandler);
                    }
                });
        }

        [LuaBridge("giantVoice_resumeVoice", 0, 0)]
        public static void ResumeVoice(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var resumeVoiceCompletedHandler = parameters.Length > 0 ? parameters[0] : null;
            giantVoice.ResumeVoice(
                delegate () {
                    if (null != resumeVoiceCompletedHandler)
                    {
                        executer.Call(resumeVoiceCompletedHandler);
                    }
                });
        }

        [LuaBridge("giantVoice_stopVoice", 0, 0)]
        public static void StopVoice(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var stopVoiceCompletedHandler = parameters.Length > 0 ? parameters[0] : null;
            giantVoice.StopVoice(
                delegate () {
                    if (null != stopVoiceCompletedHandler)
                    {
                        executer.Call(stopVoiceCompletedHandler);
                    }
                });
        }

        [LuaBridge("giantVoice_setPlayVolume", 1, 0)]
        public static void SetPlayVolume(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var volume_obj = parameters[0];
            if (null != volume_obj)
            {
                var volume = System.Convert.ToSingle(volume_obj);
                if(volume >= 0 && volume <= 1)
                {
                    giantVoice.SetPlayVolume(volume);
                }
            }
        }

        [LuaBridge("giantVoice_getCurrentPlayPosition", 0, 1)]
        public static void GetCurrentPlayPosition(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            executer.PushResult(giantVoice.GetCurrentPlayPosition());
        }

        [LuaBridge("giantVoice_isRecording", 0, 1)]
        public static void IsRecording(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            executer.PushResult(giantVoice.IsRecording());
        }

        [LuaBridge("giantVoice_isVoicePlaying", 0, 1)]
        public static void IsVoicePlaying(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            executer.PushResult(giantVoice.IsVoicePlaying());
        }

        [LuaBridge("giantVoice_isVoicePause", 0, 1)]
        public static void IsVoicePause(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            executer.PushResult(giantVoice.IsVoicePause());
        }*/

        [LuaBridge("giantVoice_clear", 0, 0)]
        public static void Clear(ILuaExecuter executer)
        {
            if(GiantVoiceToLua.giantVoice != null)
            {
                GiantVoiceToLua.giantVoice.Clear();
            }
        }
    }
}
