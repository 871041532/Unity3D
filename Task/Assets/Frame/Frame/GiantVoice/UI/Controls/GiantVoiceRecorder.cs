// -----------------------------------------------------------------
// File:    GiantVoicePlayer.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;
using GameBox.Service.UI;

namespace GameBox.Service.GiantVoice
{
    class GiantVoiceRecorder : Element, IGiantVoiceRecorder
    {
        public GiantVoiceRecorder(string path, RectTransform transform)
             :base(path, transform, UIType.GIANTVOICERECORDER)
        {
            this.recorder = this.transform.GetComponent<UI.GiantVoiceRecorder>();
        }

        public void RegisterStop(Action<string, float, string> recordSuccessfulCallback, Action<string> recordFailCallback = null)
        {
            this.recorder.RecordSuccessfulCallback = recordSuccessfulCallback;
            this.recorder.RecordFailCallback = recordFailCallback;
        }

        public void RegisterCancel(Action cancelSuccessfulCallback = null, Action<string> cancelFailCallback = null)
        {
            this.recorder.CancelSuccessfulCallback = cancelSuccessfulCallback;
            this.recorder.CancelFailCallback = cancelFailCallback;
        }
        private UI.GiantVoiceRecorder recorder = null;
    }
}