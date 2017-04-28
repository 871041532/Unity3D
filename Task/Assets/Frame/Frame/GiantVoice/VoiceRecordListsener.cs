// -----------------------------------------------------------------
// File:    VoiceRecordListsener.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------

namespace GameBox.Service.GiantVoice
{
    class VoiceRecordListsener : IRecorderListener
    {
        private GiantVoice giantVoice;
        public VoiceRecordListsener(GiantVoice giantVoice)
        {
            this.giantVoice = giantVoice;
        }
        public void onRecError(string error)
        {
            giantVoice.SetRecordSolo(false);
            if (null != giantVoice.RecordFailCallback)
            {
                giantVoice.RecordFailCallback(string.Format("[onRecError]{0}", error));
                giantVoice.RecordFailCallback = null;
            }
        }

        public void onRecStart()
        {
            
        }

        public void onRecStop(float recordTime, string file)
        {
            giantVoice.SetRecordSolo(false);
            if(giantVoice.CancelRecordVoiceFlag)
            {
                giantVoice.CancelRecordVoiceFlag = false;
                if (null != giantVoice.RecordFailCallback)
                {
                    giantVoice.RecordFailCallback("[onRecStop]cancelRecordVoice");
                    giantVoice.RecordFailCallback = null;
                }
                if(null != giantVoice.CancelSuccessfulCallback)
                {
                    giantVoice.CancelSuccessfulCallback();
                    giantVoice.CancelSuccessfulCallback = null;
                }
            }
            else
            {
                if(null != giantVoice.CancelFailCallback)
                {
                    giantVoice.CancelFailCallback("[onRecStop]failCancelRecordVoice");
                    giantVoice.CancelFailCallback = null;
                }
                giantVoice.RecordTime = recordTime;
                giantVoice.UploadVoiceFile(file);
            }
        }

        public void onRecTimeRemain(float time)
        {
            
        }

        public  void onRecTimeUp()
        {
            giantVoice.SetRecordSolo(false);
        }

        public void onRecVolume(double amplitude)
        {
            
        }
    }
}