// -----------------------------------------------------------------
// File:    ConsoleContext.cs
// Author:  mouguangyi
// Date:    2017.01.20
// Description:
//      
// -----------------------------------------------------------------
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GameBox.Framework
{
    sealed class ConsoleContext : IConsoleContext
    {
        public ConsoleContext(HttpListenerContext context)
        {
            this.context = context;
            this.currentRoute = 0;
            this.path = WWW.UnEscapeURL(context.Request.Url.AbsolutePath);
            if ("/" == this.path) {
                this.path = "/index.html?";
            }
        }

        public HttpListenerRequest Request
        {
            get {
                return this.context.Request;
            }
        }

        public HttpListenerResponse Response
        {
            get {
                return this.context.Response;
            }
        }

        public int CurrentRoute
        {
            get {
                return this.currentRoute;
            }
            set {
                this.currentRoute = value;
            }
        }

        public string Path
        {
            get {
                return this.path;
            }
        }

        public Match Match
        {
            set {
                this.match = value;
            }
        }

        public bool Pass
        {
            get {
                return this.pass;
            }
        }

        private HttpListenerContext context = null;
        private int currentRoute = 0;
        private string path = null;
        private Match match = null;
        private bool pass = false;
    }
}