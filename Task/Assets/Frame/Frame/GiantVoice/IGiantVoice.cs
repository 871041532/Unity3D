// -----------------------------------------------------------------
// File:    IGiantVoice.cs
// Author:  liuwei
// Date:    2017.02.07
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.GiantVoice
{
    public interface IGiantVoice: IService
    {
        //void StartRecordVoice();

        //void StopRecordVoice(Action<string, float, string> recordSuccessfulCallback, Action<string> recordFailCallback = null);

        //void CancelRecordVoice(Action cancelSuccessfulCallback = null, Action<string> cancelFailCallback = null);

        //void PlayVoice(string voiceId, Action playSuccessfulCallback = null, Action<string> playFailCallback = null);

        //void PauseVoice(Action pauseVoiceCompletedCallback = null);

        //void ResumeVoice(Action resumeVoiceCompletedCallback = null);

        //void StopVoice(Action stopVoiceCompletedCallback = null);

        //void SetPlayVolume(float volume);

        //int GetCurrentPlayPosition();

        //bool IsRecording();

        //bool IsVoicePlaying();

        //bool IsVoicePause();

        void Clear();
    }
}