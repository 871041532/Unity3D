// -----------------------------------------------------------------
// File:    GiantVoicePlayer.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;
using GameBox.Service.UI;
using System;

namespace GameBox.Service.GiantVoice
{
    class GiantVoicePlayer : Element, IGiantVoicePlayer
    {
        public GiantVoicePlayer(string path, RectTransform transform)
            : base(path, transform, UIType.GIANTVOICEPLAYER)
        {
            this.player = this.transform.GetComponent<UI.GiantVoicePlayer>();
        }

        public void RegisterPlay(string voiceId, Action playSuccessfulCallback = null, Action<string> playFailCallback = null)
        {
            this.player.VoiceId = voiceId;
            this.player.PlaySuccessfulCallback = playSuccessfulCallback;
            this.player.PlayFailCallback = playFailCallback;
        }

        public void RegisterStop(Action stopVoiceCompletedCallback = null)
        {
            this.player.StopVoiceCompletedCallback = stopVoiceCompletedCallback;
        }

        public void SetVoiceWords(string words)
        {
            this.player.WordText.text = words;
        }

        public void SetRecordTime(float recordTime, string format)
        {
            this.player.TimeText.text = Math.Floor(recordTime) + format;
        }

        public void SetVoiceBarLength(float length)
        {
            var trans = this.player.GetComponent<RectTransform>();
            trans.sizeDelta = new Vector2(length, trans.rect.size.y);
        }

        private UI.GiantVoicePlayer player = null;
    }
}