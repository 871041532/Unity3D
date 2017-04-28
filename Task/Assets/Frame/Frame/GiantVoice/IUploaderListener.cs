// -----------------------------------------------------------------
// File:    IUploaderListener.cs
// Author:  liuwei
// Date:    2017.03.02
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantVoice
{
    public interface IUploaderListener
    {
        void onUploadSuccess(string file, string url);
        void onUploadFailed(string file, string msg);
    }
}
