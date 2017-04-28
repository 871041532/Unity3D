// -----------------------------------------------------------------
// File:    Console.cs
// Author:  mouguangyi
// Date:    2017.01.20
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using UnityEngine;

namespace GameBox.Framework
{
    sealed class Console
    {
        private Console()
        {
            this.running = false;
        }

        private void _Log(string scope, string message, LogLevel level)
        {
            this.logs.Add(new _LogPack() { Scope = scope, Message = message, Level = level });
            if (this.logs.Count > MAXLOGCOUNT) {
                this.logs.RemoveAt(0);
            }
        }

        private void _SetLogScope(string scope, bool enabled)
        {
            this.ignoreScopes[scope] = !enabled;
        }

        private void _SetLogLevel(LogLevel logLevel)
        {
            this.logLevel = logLevel;
        }

        private bool _IsScopeEnabled(string scope)
        {
            var ignored = true;
            this.ignoreScopes.TryGetValue(scope, out ignored);

            return !ignored;
        }

        private void _Start(int port)
        {
            _RegisterRoutes();

            this.listener = new HttpListener();
            this.listener.Prefixes.Add("http://*:" + port + "/");
            this.listener.Start();
            this.listener.BeginGetContext(_ListenerCallback, null);
            this.running = true;
        }

        private void _Stop()
        {
            this.listener.Stop();
            this.listener = null;
        }

        private void _ListenerCallback(IAsyncResult result)
        {
            var context = new ConsoleContext(this.listener.EndGetContext(result));
            _HandleRequest(context);

            this.listener.BeginGetContext(new AsyncCallback(_ListenerCallback), null);
        }

        private void _HandleRequest(ConsoleContext context)
        {
            try {
                var handled = false;
                for (; context.CurrentRoute < this.registeredRoutes.Count; ++context.CurrentRoute) {
                    var route = this.registeredRoutes[context.CurrentRoute];
                    var match = route.route.Match(context.Path);
                    if (!match.Success) {
                        continue;
                    }

                    if (!route.methods.IsMatch(context.Request.HttpMethod)) {
                        continue;
                    }

                    // TODO: Main thread

                    context.Match = match;
                    route.callback(context);
                    handled = !context.Pass;
                    if (handled) {
                        break;
                    }
                }

                if (!handled) {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.StatusDescription = "Not Found";
                }
            } catch (Exception exception) {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusDescription = string.Format("Fatal error:\n{0}", exception);

                Debug.LogException(exception);
            }

            context.Response.OutputStream.Close();
        }

        private void _RegisterRoutes()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static)) {
                    var attrs = method.GetCustomAttributes(typeof(ConsoleRouteAttribute), true) as ConsoleRouteAttribute[];
                    if (0 == attrs.Length) {
                        continue;
                    }

                    var callback = Delegate.CreateDelegate(typeof(ConsoleRouteAttribute.Callback), method, false) as ConsoleRouteAttribute.Callback;
                    if (null == callback) {
                        continue;
                    }

                    foreach (var route in attrs) {
                        if (null == route.route) {
                            continue;
                        }

                        route.callback = callback;
                        this.registeredRoutes.Add(route);
                    }
                }
            }
        }

        private struct _LogPack
        {
            public string Scope;
            public string Message;
            public LogLevel Level;
        }

        private bool running = false;
        private HttpListener listener = null;
        private List<ConsoleRouteAttribute> registeredRoutes = new List<ConsoleRouteAttribute>();
        private List<_LogPack> logs = new List<_LogPack>();
        private Dictionary<string, bool> ignoreScopes = new Dictionary<string, bool>();
        private LogLevel logLevel = LogLevel.VERBOSE;

        private const int MAXLOGCOUNT = 100;

        public static void L(string scope, string message)
        {
            if (_Instance.running) {
                _Instance._Log(scope, message, LogLevel.VERBOSE);
            }
        }

        public static void W(string scope, string message)
        {
            if (_Instance.running) {
                _Instance._Log(scope, message, LogLevel.WARNING);
            }
        }

        public static void E(string scope, string message)
        {
            if (_Instance.running) {
                _Instance._Log(scope, message, LogLevel.ERROR);
            }
        }

        public static void X(string scope, string message)
        {
            if (_Instance.running) {
                _Instance._Log(scope, message, LogLevel.EXCEPTION);
            }
        }

        public static void Start(int port)
        {
            if (!_Instance.running) {
                _Instance._Start(port);
            }
        }

        public static void Stop()
        {
            if (_Instance.running) {
                _Instance._Stop();
            }
        }

        [ConsoleRoute("/index.html?")]
        public static void IndexHTMLContent(IConsoleContext context)
        {
            context.Response.WriteString(IndexHTML.CONTENT, "text/html");
        }

        [ConsoleRoute("^/console/output$")]
        public static void Output(IConsoleContext context)
        {
            var content = "";
            for (var i = 0; i < _Instance.logs.Count; ++i) {
                var log = _Instance.logs[i];
                if (_Instance._IsScopeEnabled(log.Scope) && log.Level <= _Instance.logLevel) {
                    switch (log.Level) {
                    case LogLevel.VERBOSE:
                        content += "<li class='list-group-item list-group-item-info'><span class='glyphicon glyphicon-info-sign' aria-hidden='true'></span> [" + log.Scope + "] " + log.Message + "</li>";
                        break;
                    case LogLevel.WARNING:
                        content += "<li class='list-group-item list-group-item-warning'><span class='glyphicon glyphicon-exclamation-sign' aria-hidden='true'></span> [" + log.Scope + "] " + log.Message + "</li>";
                        break;
                    case LogLevel.ERROR:
                        content += "<li class='list-group-item list-group-item-danger'><span class='glyphicon glyphicon-remove-sign' aria-hidden='true'></span> [" + log.Scope + "] " + log.Message + "</li>";
                        break;
                    case LogLevel.EXCEPTION:
                        content += "<li class='list-group-item list-group-item-danger'><span class='glyphicon glyphicon-flash' aria-hidden='true'></span> [" + log.Scope + "] " + log.Message + "</li>";
                        break;
                    }
                }
            }

            context.Response.WriteString(content);
        }

        [ConsoleRoute("^/console/logscope$")]
        public static void SetLogScope(IConsoleContext context)
        {
            foreach (string scope in context.Request.QueryString) {
                var enabled = bool.Parse(context.Request.QueryString.Get(scope));
                _Instance._SetLogScope(scope, enabled);
                break;
            }
        }

        [ConsoleRoute("^/console/loglevel$")]
        public static void SetLogLevel(IConsoleContext context)
        {
            var level = int.Parse(context.Request.QueryString.Get(0));
            _Instance._SetLogLevel((LogLevel)level);
        }

        private static Console _Instance
        {
            get {
                if (null == Console.instance) {
                    Console.instance = new Console();
                }

                return Console.instance;
            }
        }

        private static Console instance = null;
    }
}