// -----------------------------------------------------------------
// File:    VoiceDownloadListener.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------

namespace GameBox.Service.GiantVoice
{
    class VoiceDownloadListener : IDownloaderListener
    {
        private GiantVoice giantVoice;
        public VoiceDownloadListener(GiantVoice giantVoice)
        {
            this.giantVoice = giantVoice;
        }
        public void onDownloadSuccess(string url, string file)
        {
            giantVoice.playVoice(url);
        }
        public void onDownloadFailed(string url, string msg)
        {
            if(null != giantVoice.PlayFailCallback)
            {
                giantVoice.PlayFailCallback(string.Format("[onDownloadFailed]{0}:{1}", url, msg));
                giantVoice.PlayFailCallback = null;
            }
        }
    }
}