// -----------------------------------------------------------------
// File:    VoiceUploadListener.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------

namespace GameBox.Service.GiantVoice
{
    class VoiceUploadListener : IUploaderListener
    {
        private GiantVoice giantVoice;
        public VoiceUploadListener(GiantVoice giantVoice)
        {
            this.giantVoice = giantVoice;
        }
        public void onUploadSuccess(string file, string url)
        {
            giantVoice.VoiceId = url;
            giantVoice.ConvertVoiceToWords(file);
        }
        public void onUploadFailed(string file, string msg)
        {
            if(null != giantVoice.RecordFailCallback)
            {
                giantVoice.RecordFailCallback(string.Format("[onUploadFailed]{0}:{1}", file, msg));
                giantVoice.RecordFailCallback = null;
            }
        }
    }
}