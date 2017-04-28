// -----------------------------------------------------------------
// File:    IPlayerListener.cs
// Author:  liuwei
// Date:    2017.03.02
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantVoice
{
    public interface IPlayerListener
    {
        void onPlayError(string error);
        void onPlayStart();
        void onPlayPause();
        void onPlayResume();
        void onPlayStop();
        void onPlayCompleted();
    }
}
