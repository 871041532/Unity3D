// -----------------------------------------------------------------
// File:    IGameAnalytics.cs
// Author:  liuwei
// Date:    2017.03.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System.Collections.Generic;

namespace GameBox.Service.GameAnalytics
{
    public interface IGameAnalytics : IService
    {
        void OnEvent(string eventId, Dictionary<string, object> dictionary);
    }
}