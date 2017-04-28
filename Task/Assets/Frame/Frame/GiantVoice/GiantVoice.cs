// -----------------------------------------------------------------
// File:    GiantVoice.cs
// Author:  liuwei
// Date:    2017.02.07
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using GameBox.Service.UI;
using System;
using UnityEngine;

namespace GameBox.Service.GiantVoice
{
    sealed class GiantVoice : IGiantVoice, IControlFactory
    {
        /// <summary>
        /// 录音时间
        /// </summary>
        public float RecordTime;
        /// <summary>
        /// 语音id
        /// </summary>
        public string VoiceId;

        /// <summary>
        /// 录音成功返回voiceId、recordTime、words
        /// </summary>
        public Action<string, float, string> RecordSuccessfulCallback = null;
        /// <summary>
        /// 录音失败返回错误码
        /// <summary>
        public Action<string> RecordFailCallback = null;

        /// <summary>
        /// 播放成功返回
        /// <summary>
        public Action PlaySuccessfulCallback = null;
        /// <summary>
        /// 播放失败返回错误码
        /// <summary>
        public Action<string> PlayFailCallback = null;

        /// <summary>
        /// 取消录音成功返回
        /// <summary>
        public Action CancelSuccessfulCallback = null;
        /// <summary>
        /// 取消录音失败返回错误码
        /// <summary>
        public Action<string> CancelFailCallback = null;

        public bool CancelRecordVoiceFlag = false;

        /// <summary>
        /// 暂停播放完成返回
        /// <summary>
        public Action PauseVoiceCompletedCallback = null;
        /// <summary>
        /// 恢复播放完成返回
        /// <summary>
        public Action ResumeVoiceCompletedCallback = null;
        /// <summary>
        /// 停止播放完成返回
        /// <summary>
        public Action StopVoiceCompletedCallback = null;

        public GiantVoice()
        {

        }

        public string Id
        {
            get
            {
                return "com.giant.service.giantvoice";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServicesTask(new string[] {
                "com.giant.service.nativechannel",
                "com.giant.service.uisystem"
            }).Start().Continue(task =>
            {
                var services = task.Result as IService[];
                var nativeChannel = services[0] as INativeChannel;
                this.proxy = nativeChannel.Connect(new NativeModule
                {
                    IOS = "IOSGiantVoiceWrapper",
                    Android = "com.giant.giantvoice.GiantVoiceWrapper",
                }, this);


                var system = services[1] as IUISystem;
                system.RegisterFactory(this);

                var gameId = runner.GetArgs<int>("GameId");//游戏应用id
                var userId = runner.GetArgs<long>("UserId");//用户id
                var maxTime = runner.GetArgs<float>("MaxTime");
                var minTime = runner.GetArgs<float>("MinTime");
                var gameName = runner.GetArgs<string>("GameName");//游戏名
                var zoneId = runner.GetArgs<string>("ZoneId");//服务器区id
                var androidAppId = runner.GetArgs<string>("AndroidAppId");
                var iosAppId = runner.GetArgs<string>("IosAppId");
                var httpServer = runner.GetArgs<string>("HttpServer");

                this.proxy.Call("SetHttpServer", httpServer);
                this.proxy.Call("Initialize", gameId, 100, userId, gameName, zoneId);
                this.proxy.Call("SetRecordMaxTime", maxTime);
                this.proxy.Call("SetRecordMinTime", minTime);

                if (Application.platform == RuntimePlatform.Android)
                {
                    this.proxy.Call("EnableVoiceToWord", androidAppId);
                }
                else
                {
                    this.proxy.Call("EnableVoiceToWord", iosAppId);
                }
                this.proxy.Call("SetVoiceLanguage", "zh_cn");
                this.proxy.Call("DeleteAllGCloudFile");

                this.recorderListener = new VoiceRecordListsener(this);
                this.playerListener = new VoicePlayerListener(this);
                this.converterListener = new VoiceToWordListener(this);
                this.voiceUploadListener = new VoiceUploadListener(this);
                this.voiceDownloadListener = new VoiceDownloadListener(this);

                runner.Ready(_Terminate);
                return null;
            });
        }

        public void Pulse(float delta)
        {

        }

        private void _Terminate()
        {

        }

        public void StartRecordVoice()
        {
            this.SetRecordSolo(true);
            this.proxy.Call("StartRecord");
        }

        public void StopRecordVoice(Action<string, float, string> recordSuccessfulCallback, Action<string> recordFailCallback = null)
        {
            this.proxy.Call("StopRecord");
            this.RecordSuccessfulCallback = recordSuccessfulCallback;
            this.RecordFailCallback = recordFailCallback;
        }

        public void CancelRecordVoice(Action cancelSuccessfulCallback = null, Action<string> cancelFailCallback = null)
        {
            this.CancelSuccessfulCallback = cancelSuccessfulCallback;
            this.CancelFailCallback = cancelFailCallback;
            CancelRecordVoiceFlag = true;
            this.proxy.Call("CancelRecord");
            this.SetRecordSolo(false);
        }

        public void PlayVoice(string voiceId, Action playSuccessfulCallback = null, Action<string> playFailCallback = null)
        {
            this.PlaySuccessfulCallback = playSuccessfulCallback;
            this.PlayFailCallback = playFailCallback;
            playVoice(voiceId);
        }

        public void PauseVoice(Action pauseVoiceCompletedCallback = null)
        {
            this.PauseVoiceCompletedCallback = pauseVoiceCompletedCallback;
            this.proxy.Call("PauseVoice");
        }

        public void ResumeVoice(Action resumeVoiceCompletedCallback = null)
        {
            this.ResumeVoiceCompletedCallback = resumeVoiceCompletedCallback;
            this.proxy.Call("ResumeVoice");
        }

        public void StopVoice(Action stopVoiceCompletedCallback = null)
        {
            this.StopVoiceCompletedCallback = stopVoiceCompletedCallback;
            this.proxy.Call("StopVoice");
        }

        public void SetPlayVolume(float volume)
        {
            this.proxy.Call("SetPlayVolume", volume);
        }

        /*public int GetCurrentPlayPosition()
        {
            return this.voice.GetCurrentPlayPosition();
        }*/

        public bool IsRecording()
        {
            return this.isRecording;
        }

        public bool IsVoicePause()
        {
            return this.isPause;
        }

        public bool IsVoicePlaying()
        {
            return this.isPlaying;
        }

        public void Clear()
        {
            this.proxy.Call("Destroy");
            this.proxy.Call("ClearAllAudioFile");
        }

        public void ClearSelf()
        {
            this.proxy.Call("Destroy");
        }

        public void ClearAllAudioFile()
        {
            this.proxy.Call("ClearAllAudioFile");
        }

        public void ClearOldAudioFiles()
        {
            this.proxy.Call("ClearOldAudioFiles");
        }

        public void UploadVoiceFile(string fileName)
        {
            this.proxy.Call("UploadFile", fileName);
        }

        public void ConvertVoiceToWords(string fileName)
        {
            this.proxy.Call("ConvertVoiceToWords", fileName);
        }

        public void playVoice(string url)
        {
            this.proxy.Call("PlayVoice", url);
        }

        public void SetRecordSolo(bool state)
        {
            this.proxy.Call("SetRecordSolo", state);
        }

        public IElement Create(int type, string path, RectTransform transform)
        {
            switch (type)
            {
                case UIType.GIANTVOICEPLAYER:
                    return new GiantVoicePlayer(path, transform);
                case UIType.GIANTVOICERECORDER:
                    return new GiantVoiceRecorder(path, transform);
                default:
                    return null;
            }
        }


        //--start--------IRecorderListener------------
        private void onRecordStart()
        {
            setIsRecording(true);
            this.recorderListener.onRecStart();
        }

        private void onRecordStop(double recordTime, string file)
        {
            setIsRecording(false);
            this.recorderListener.onRecStop((float)recordTime, file);
        }

        private void onRecordError(string error)
        {
            setIsRecording(false);
            this.recorderListener.onRecError(error);
        }

        private void onRecordTimeRemain(double time)
        {
            //setIsRecording(true);
            this.recorderListener.onRecTimeRemain((float)time);
        }

        private void onRecordTimeUp()
        {
            setIsRecording(false);
            this.recorderListener.onRecTimeUp();
        }

        private void onRecordVolume(double amplitude)
        {
            this.recorderListener.onRecVolume(amplitude);
        }
        //--end---------IRecorderListener----------


        //--start----------IPlayerListener------------
        private void onPlayError(string error)
        {
            setIsVoicePlaying(false);
            this.playerListener.onPlayError(error);
        }

        private void onPlayStart()
        {
            setIsVoicePlaying(true);
            this.playerListener.onPlayStart();
        }

        private void onPlayPause()
        {
            setIsVoicePause(true);
            this.playerListener.onPlayPause();
        }

        private void onPlayResume()
        {
            setIsVoicePause(false);
            this.playerListener.onPlayResume();
        }

        private void onPlayStop()
        {
            setIsVoicePlaying(false);
            this.playerListener.onPlayStop();
        }

        private void onPlayCompleted()
        {
            setIsVoicePlaying(false);
            this.playerListener.onPlayCompleted();
        }
        //--end---------IPlayerListener----------

        //--start----------IConverterListener--------
        private void onConvertSuccess(string file, string words)
        {
            this.converterListener.onConvertSuccess((string)file, (string)words);
        }

        private void onConvertError(string file, string error)
        {
            this.converterListener.onConvertError((string)file, (string)error);
        }
        //--end---------IConverterListener----------

        //--start----------IUploaderListener--------
        private void onUploadSuccess(string file, string url)
        {
            this.voiceUploadListener.onUploadSuccess(file, url);
        }

        private void onUploadFailed(string file, string msg)
        {
            this.voiceUploadListener.onUploadFailed(file, msg);
        }
        //--end---------IUploaderListener----------

        //--start----------IDownloaderListener--------
        private void onDownloadSuccess(string url, string file)
        {
            this.voiceDownloadListener.onDownloadSuccess(url, file);
        }

        private void onDownloadFailed(string url, string msg)
        {
            this.voiceDownloadListener.onDownloadFailed(url, msg);
        }
        //--end---------IDownloaderListener----------

        private void setIsVoicePlaying(bool state)
        {
            this.isPlaying = state;
        }

        private void setIsRecording(bool state)
        {
            this.isRecording = state;
        }
        private void setIsVoicePause(bool state)
        {
            this.isPause = state;
        }

        private IRecorderListener recorderListener;
        private IPlayerListener playerListener;
        private IConverterListener converterListener;
        private IUploaderListener voiceUploadListener;
        private IDownloaderListener voiceDownloadListener;

        private INativeProxy proxy;

        private bool isPlaying = false;
        private bool isRecording = false;
        private bool isPause = false;
    }
}