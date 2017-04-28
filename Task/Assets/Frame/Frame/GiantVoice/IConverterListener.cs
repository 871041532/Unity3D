// -----------------------------------------------------------------
// File:    IConverterListener.cs
// Author:  liuwei
// Date:    2017.03.02
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.GiantVoice
{
    public interface IConverterListener
    {
        void onConvertError(string file, string error);
        void onConvertSuccess(string file, string words);
    }
}