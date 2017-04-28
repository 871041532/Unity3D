// -----------------------------------------------------------------
// File:    IRecorderListener.cs
// Author:  liuwei
// Date:    2017.03.02
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantVoice
{
    public interface IRecorderListener
    {
        void onRecStart();
        void onRecStop(float recordTime, string file);
        void onRecError(string error);
        void onRecTimeRemain(float time);
        void onRecTimeUp();
        void onRecVolume(double amplitude);
    }
}