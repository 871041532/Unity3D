// ---------------------------------------------------------------
// File: ShinyGeoQueryEngine.cs
// Author: mouguangyi
// Date: 2016.03.24
// Description:
//   Provide Geo query service based on geoip.nekudo.com (http://geoip.nekudo.com/).
// ---------------------------------------------------------------
using GameBox.Framework;
using TinyJSON;
using UnityEngine;

namespace GameBox.Service.GeoQuery
{
    class ShinyGeoQueryEngine : IGeoQueryEngine
    {
        public ShinyGeoQueryEngine()
        { }

        public AsyncTask SimpleQuery(string ip)
        {
            return new WWWLoadTask(SERVICE_URL + (string.IsNullOrEmpty(ip) ? "" : ip), 5).Start().Continue(task =>
            {
                WWW result = task.Result as WWW;
                if (null == result) {
                    return _ErrorResult("Time out");
                } else if (!string.IsNullOrEmpty(result.error)) {
                    return _ErrorResult(result.error);
                } else {
                    var jsonNode = new Parser().Load(result.text);
                    return new GeoResult {
                        Status = 0,
                        Error = "",
                        CountryCode = jsonNode["country"]["code"].ToString(),
                        Country = jsonNode["country"]["name"].ToString(),
                        Subdivision = "",
                        City = jsonNode["city"].ToString(),
                    };
                }
            });
        }

        public AsyncTask Query(string ip)
        {
            if (string.IsNullOrEmpty(ip)) {
                return new WWWLoadTask(SERVICE_URL, 5).Start().Continue(task =>
                {
                    return _HandleQueryIP(task);
                }).Continue(task =>
                {
                    return _HandleQueryFull(task);
                });
            } else {
                return new WWWLoadTask(SERVICE_URL + ip + "/full", 5).Start().Continue(task =>
                {
                    return _HandleQueryFull(task);
                });
            }
        }

        private object _HandleQueryIP(AsyncTask task)
        {
            WWW result = task.Result as WWW;
            if (null == result) {
                return _ErrorResult("Time out");
            } else if (!string.IsNullOrEmpty(result.error)) {
                return _ErrorResult(result.error);
            } else {
                var jsonNode = new Parser().Load(result.text);
                return new WWWLoadTask(SERVICE_URL + jsonNode["ip"].ToString() + "/full", 5);
            }
        }

        private object _HandleQueryFull(AsyncTask task)
        {
            WWW result = task.Result as WWW;
            if (null == result) {
                return _ErrorResult("Time out");
            } else if (!string.IsNullOrEmpty(result.error)) {
                return _ErrorResult(result.error);
            } else {
                var jsonNode = new Parser().Load(result.text);
                return new GeoResult {
                    Status = 0,
                    Error = "",
                    CountryCode = jsonNode["country"]["iso_code"].ToString(),
                    Country = jsonNode["country"]["names"]["zh-CN"].ToString(),
                    Subdivision = jsonNode["subdivisions"][0]["names"]["zh-CN"].ToString(),
                    City = jsonNode["city"]["names"]["zh-CN"].ToString(),
                };
            }
        }

        private IGeoResult _ErrorResult(string error)
        {
            return new GeoResult {
                Status = -1,
                Error = error,
                CountryCode = null,
                Subdivision = null,
                City = null,
            };
        }

        private const string SERVICE_URL = "http://geoip.nekudo.com/api/";
    }
}
