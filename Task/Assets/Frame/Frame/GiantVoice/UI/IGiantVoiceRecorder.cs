// -----------------------------------------------------------------
// File:    IGiantVoiceRecorder.cs
// Author:  liuwei
// Date:    2017.02.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.UI;
using System;

namespace GameBox.Service.GiantVoice
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGiantVoiceRecorder : IElement
    {
        void RegisterStop(Action<string, float, string> recordSuccessfulCallback, Action<string> recordFailCallback = null);

        void RegisterCancel(Action cancelSuccessfulCallback = null, Action<string> cancelFailCallback = null);
    }
}
