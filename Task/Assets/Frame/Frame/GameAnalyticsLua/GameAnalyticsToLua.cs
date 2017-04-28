// -----------------------------------------------------------------
// File:    GameAnalyticsToLua.cs
// Author:  liuwei
// Date:    2017.02.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.GameAnalytics;
using GameBox.Service.LuaRuntime;
using System;
using System.Collections.Generic;

namespace GameBox.Service.GameAnalyticsLua
{
    class GameAnalyticsToLua : IGameAnalyticsToLua
    {
        public static IGameAnalytics gameAnalytics;
        public string Id
        {
            get
            {
                return "com.giant.service.gameanalyticstolua";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServicesTask(new string[]{
                "com.giant.service.gameanalytics",
                "com.giant.service.luaruntime",
            }).Start().Continue(task =>
            {
                var services = task.Result as IService[];
                gameAnalytics = services[0] as IGameAnalytics;
                var luaRuntime = services[1] as ILuaRuntime;

                GameAnalyticsToLua.RegLuaBridgeFunction(luaRuntime);

                runner.Ready(_Terminate);
                return null;
            });
        }

        public void Pulse(float delata)
        { }

        private void _Terminate()
        { }

        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(GameAnalyticsToLua.OnEvent);
        }


        [LuaBridge("gameAnalytics_onEvent", 2, 0)]
        public static void OnEvent(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var eventId = parameters[0].ToString();
            var luaTable = parameters[1] as ILuaTable;
            var map = luaTable.ToMap();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            /*Dictionary<object, object>.Enumerator enumerator = map.GetEnumerator();
            while(enumerator.MoveNext())
            {
                dictionary.Add(enumerator.Current.Key.ToString(), enumerator.Current.Value.ToString());
            }*/
            foreach (var v in map)
            {
                object o;
                if (v.Value is ILuaString)
                {
                    o = v.Value.ToString();
                }
                else
                {
                    o = v.Value;
                }
                dictionary.Add(v.Key.ToString(), o);
            }
            if (GameAnalyticsToLua.gameAnalytics != null)
            {
                GameAnalyticsToLua.gameAnalytics.OnEvent(eventId, dictionary);
            }
        }
    }
}
