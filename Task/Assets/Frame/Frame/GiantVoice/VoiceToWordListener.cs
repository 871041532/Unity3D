// -----------------------------------------------------------------
// File:    VoiceToWordListener.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------

namespace GameBox.Service.GiantVoice
{
    class VoiceToWordListener : IConverterListener
    {
        private GiantVoice giantVoice;
        public VoiceToWordListener(GiantVoice giantVoice)
        {
            this.giantVoice = giantVoice;
        }
        public void onConvertSuccess(string file, string words)
        {
            if (null != giantVoice.RecordSuccessfulCallback)
            {
                giantVoice.RecordSuccessfulCallback(giantVoice.VoiceId, giantVoice.RecordTime, words);
                giantVoice.RecordSuccessfulCallback = null;
            }
        }

        public void onConvertError(string file, string error)
        {
            if (null != giantVoice.RecordFailCallback)
            {
                giantVoice.RecordFailCallback(string.Format("[onConvertError]{0}:{1}", file, error));
                giantVoice.RecordFailCallback = null;
            }
        }
    }
}