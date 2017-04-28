// -----------------------------------------------------------------
// File:    IGiantVoicePlayer.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.UI;
using System;

namespace GameBox.Service.GiantVoice
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGiantVoicePlayer : IElement
    {
        void RegisterPlay(string voiceId, Action playSuccessfulCallback = null, Action<string> playFailCallback = null);

        void RegisterStop(Action stopVoiceCompletedCallback = null);

        void SetVoiceWords(string words);

        void SetRecordTime(float recordTime, string format);

        void SetVoiceBarLength(float length);
    }
}