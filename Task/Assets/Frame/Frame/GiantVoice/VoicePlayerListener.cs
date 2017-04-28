// -----------------------------------------------------------------
// File:    VoicePlayerListener.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------

namespace GameBox.Service.GiantVoice
{
    class VoicePlayerListener : IPlayerListener
    {
        private GiantVoice giantVoice;
        public VoicePlayerListener(GiantVoice giantVoice)
        {
            this.giantVoice = giantVoice;
        }
        public void onPlayCompleted()
        {
            if(null != giantVoice.PlaySuccessfulCallback)
            {
                giantVoice.PlaySuccessfulCallback();
                giantVoice.PlaySuccessfulCallback = null;
            }
        }

        public void onPlayError(string error)
        {
            if (null != giantVoice.PlayFailCallback)
            {
                giantVoice.PlayFailCallback(string.Format("[onPlayError]{0}", error));
                giantVoice.PlayFailCallback = null;
            }
        }

        public void onPlayPause()
        {
            if (null != giantVoice.PauseVoiceCompletedCallback)
            {
                giantVoice.PauseVoiceCompletedCallback();
                giantVoice.PauseVoiceCompletedCallback = null;
            }
        }

        public void onPlayResume()
        {
            if (null != giantVoice.ResumeVoiceCompletedCallback)
            {
                giantVoice.ResumeVoiceCompletedCallback();
                giantVoice.ResumeVoiceCompletedCallback = null;
            }
        }

        public void onPlayStart()
        {
            
        }

        public void onPlayStop()
        {
            if (null != giantVoice.StopVoiceCompletedCallback)
            {
                giantVoice.StopVoiceCompletedCallback();
                giantVoice.StopVoiceCompletedCallback = null;
            }
        }
    }
}