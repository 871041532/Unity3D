// -----------------------------------------------------------------
// File:    GiantVoicePlayerComponent.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameBox.Service.GiantVoice.UI
{
    public class GiantVoicePlayer : MonoBehaviour
    {
        [HideInInspector]
        public Button Button;
        [HideInInspector]
        public Image ButtonImage;
        [HideInInspector]
        public Image PictureImage;
        [HideInInspector]
        public Text TimeText;
        [HideInInspector]
        public Text WordText;
        [HideInInspector]
        public bool InitFlag = false;
        [HideInInspector]
        public string VoiceId;
        [HideInInspector]
        public Action PlaySuccessfulCallback = null;
        [HideInInspector]
        public Action<string> PlayFailCallback = null;
        [HideInInspector]
        public Action StopVoiceCompletedCallback = null;

        void Start()
        {
            new ServiceTask<IGiantVoice>().Start().Continue(task =>
            {
                this.giantVoice = task.Result as GiantVoice;
                this.Button.onClick.AddListener(delegate ()
                {
                    if(this.giantVoice.IsVoicePlaying())
                    {
                        this.giantVoice.StopVoice(this.StopVoiceCompletedCallback);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.VoiceId))
                        {
                            this.giantVoice.PlayVoice(this.VoiceId, this.PlaySuccessfulCallback, this.PlayFailCallback);
                        }
                    }
                });
                return null;
            });
        }

        private GiantVoice giantVoice;
    }
}