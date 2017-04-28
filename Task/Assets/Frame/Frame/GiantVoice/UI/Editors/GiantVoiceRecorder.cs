// -----------------------------------------------------------------
// File:    GiantVoiceRecorderComponent.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameBox.Framework;

namespace GameBox.Service.GiantVoice.UI
{
    public class GiantVoiceRecorder : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
    {
        [HideInInspector]
        public Button Button;
        [HideInInspector]
        public Image Image;
        [HideInInspector]
        public Text Text;
        [HideInInspector]
        public bool InitFlag = false;
        [HideInInspector]
        public Action<string, float, string> RecordSuccessfulCallback = null;
        [HideInInspector]
        public Action<string> RecordFailCallback = null;
        [HideInInspector]
        public Action CancelSuccessfulCallback = null;
        [HideInInspector]
        public Action<string> CancelFailCallback = null;

        public void OnPointerDown(PointerEventData eventData)
        {
            if(this.giantVoice != null)
            {
                this.giantVoice.StartRecordVoice();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (this.giantVoice != null)
            {
                if(this.sendFlag)
                {
                    this.giantVoice.StopRecordVoice(
                        delegate (string voiceId, float recordTime, string words)
                        {
                            if(this.RecordSuccessfulCallback != null)
                                this.RecordSuccessfulCallback(voiceId, recordTime, words);
                        }, delegate (string error)
                        {
                            if (this.RecordFailCallback != null)
                                this.RecordFailCallback(error);
                        });
                }
                else
                {
                    this.giantVoice.CancelRecordVoice(
                        delegate ()
                        {
                            if(this.CancelSuccessfulCallback != null)
                                this.CancelSuccessfulCallback();
                        }, delegate (string error)
                        {
                            if(this.CancelFailCallback != null)
                            {
                                this.CancelFailCallback(error);
                            }
                        });
                }
                
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.sendFlag = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.sendFlag = false;
        }

        void Start()
        {
            new ServiceTask<IGiantVoice>().Start().Continue(task =>
            {
                this.giantVoice = task.Result as GiantVoice;
                return null;
            });
        }

        private GiantVoice giantVoice;
        private bool sendFlag = false;
    }
}