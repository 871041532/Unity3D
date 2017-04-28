// -----------------------------------------------------------------
// File:    IDownloaderListener.cs
// Author:  liuwei
// Date:    2017.03.02
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantVoice
{
    public interface IDownloaderListener
    {
        void onDownloadFailed(string url, string msg);
        void onDownloadSuccess(string url, string file);
    }
}
