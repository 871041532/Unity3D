// -----------------------------------------------------------------
// File:    HttpChannel.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBox.Service.NetworkManager
{
    class HttpChannel : NetworkChannel, IHttpChannel
    {
        public HttpChannel()
        { }

        public AsyncTask Request(string url, string method = "GET", IDictionary<string, object> data = null)
        {
            if (method.Equals("GET")) {
                if (null != data) {
                    var builder = new StringBuilder(url);
                    builder.Append("?");
                    foreach (var key in data.Keys) {
                        builder.Append(key);
                        builder.Append("=");
                        builder.Append(data[key].ToString());
                        builder.Append("&");
                    }
                    builder.Append(new DateTime().Millisecond);
                    url = builder.ToString();
                }

                return new WWWLoadTask(url);
            } else if (method.Equals("POST")) {
                var builder = new StringBuilder();
                foreach (var key in data.Keys) {
                    builder.Append(key);
                    builder.Append("=");
                    builder.Append(data[key].ToString());
                    builder.Append("&");
                }

                var postData = UTF8Encoding.UTF8.GetBytes(builder.ToString());
                return new WWWLoadTask(url, postData);
            } else {
                return null;
            }
        }
    }
}