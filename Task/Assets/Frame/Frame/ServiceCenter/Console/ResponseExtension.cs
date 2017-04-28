// -----------------------------------------------------------------
// File:    ResponseExtension.cs
// Author:  mouguangyi
// Date:    2017.01.22
// Description:
//      
// -----------------------------------------------------------------
using System.Net;
using System.Text;

namespace GameBox.Framework
{
    public static class ResponseExtension
    {
        public static void WriteString(this HttpListenerResponse response, string input, string type = "text/plain")
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";

            if (!string.IsNullOrEmpty(input)) {
                var buffer = Encoding.UTF8.GetBytes(input);
                response.ContentLength64 = buffer.Length;
                response.ContentType = type;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}