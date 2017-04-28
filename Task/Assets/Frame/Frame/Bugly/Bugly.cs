// -----------------------------------------------------------------
// File:    Bugly.cs
// Author:  mouguangyi
// Date:    2017.02.04
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.NativeChannel;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GameBox.Service.Bugly
{
    /// <summary>
    /// Log severity. 
    /// { Log, LogDebug, LogInfo, LogWarning, LogAssert, LogError, LogException }
    /// </summary>
    enum LogSeverity
    {
        Log,
        LogDebug,
        LogInfo,
        LogWarning,
        LogAssert,
        LogError,
        LogException
    }

    sealed class Bugly : IBugly
    {
        // IService
        public string Id
        {
            get {
                return "com.giant.service.bugly";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServiceTask<INativeChannel>().Start().Continue(task =>
            {
                var nativeChannel = task.Result as INativeChannel;
                this.proxy = nativeChannel.Connect(new NativeModule {
                    IOS = "BuglyWrapper",
                    Android = "com.giant.bugly.BuglyWrapper",
                }, this);

                string appId = "";
                switch (Application.platform) {
                case RuntimePlatform.IPhonePlayer:
                    appId = runner.GetArgs<string>("AppIdForIOS");
                    break;
                case RuntimePlatform.Android:
                    appId = runner.GetArgs<string>("AppIdForAndroid");
                    break;
                }

                if (!string.IsNullOrEmpty(appId)) {
                    _ConfigDebugMode(runner.GetArgs<bool>("IsDebug"));
                    this.proxy.Call("setGameType", GAME_TYPE_UNITY);
                    this.proxy.Call("init", appId);
                    _RegisterExceptionHandler();

                    Logger<IBugly>.L("Bugly initialized with app id: " + appId);
                }

                runner.Ready(_Terminate);

                return null;
            });
        }

        public void Pulse(float delta)
        { }

        // IBugly
        public void RegisterLogCallback(LogCallbackDelegate handler)
        {
            if (null != handler) {
                this.logCallbackEventHandler += handler;
            }
        }

        public void UnregisterLogCallback(LogCallbackDelegate handler)
        {
            if (null != handler) {
                this.logCallbackEventHandler -= handler;
            }
        }

        // -- private --
        private void _Terminate()
        {
            this.proxy.Disconnect();
            this.proxy = null;
        }

        private void _ConfigDebugMode(bool enabled)
        {
            _ConfigCrashReporter();

            this.proxy.Call("setLogEnable", enabled);
        }

        private void _RegisterExceptionHandler()
        {
            try {
                Application.logMessageReceived += _OnLogCallbackHandler;
                AppDomain.CurrentDomain.UnhandledException += _OnUncaughtExceptionHandler;

                Logger<IBugly>.L("Register the log callback in Unity " + Application.unityVersion);
            } catch { }

            _SetUnityVersion();
        }

        private void _OnLogCallbackHandler(string condition, string stackTrace, UnityEngine.LogType type)
        {
            if (this.logCallbackEventHandler != null) {
                this.logCallbackEventHandler(condition, stackTrace, type);
            }

            if (!string.IsNullOrEmpty(condition) && condition.Contains("[BuglyAgent] <Log>")) {
                return;
            }

            if (this.uncaughtAutoReportOnce) {
                return;
            }

            // convert the log level
            LogSeverity logLevel = LogSeverity.Log;
            switch (type) {
            case UnityEngine.LogType.Exception:
                logLevel = LogSeverity.LogException;
                break;
            case UnityEngine.LogType.Error:
                logLevel = LogSeverity.LogError;
                break;
            case UnityEngine.LogType.Assert:
                logLevel = LogSeverity.LogAssert;
                break;
            case UnityEngine.LogType.Warning:
                logLevel = LogSeverity.LogWarning;
                break;
            case UnityEngine.LogType.Log:
                logLevel = LogSeverity.LogDebug;
                break;
            default:
                break;
            }

            if (LogSeverity.Log == logLevel) {
                return;
            }

            _HandleException(logLevel, null, condition, stackTrace, true);
        }

        private void _OnUncaughtExceptionHandler(object sender, System.UnhandledExceptionEventArgs args)
        {
            if (args == null || args.ExceptionObject == null) {
                return;
            }

            try {
                if (args.ExceptionObject.GetType() != typeof(System.Exception)) {
                    return;
                }
            } catch {
                if (UnityEngine.Debug.isDebugBuild == true) {
                    UnityEngine.Debug.Log("BuglyAgent: Failed to report uncaught exception");
                }

                return;
            }

            if (this.uncaughtAutoReportOnce) {
                return;
            }

            _HandleException((System.Exception)args.ExceptionObject, null, true);
        }

        private void _SetUnityVersion()
        {
            _ConfigCrashReporter();

            this.proxy.Call("setUnityVersion", Application.unityVersion);
        }

        private void _HandleException(LogSeverity logLevel, string name, string message, string stackTrace, bool uncaught)
        {
            if (logLevel == LogSeverity.Log) {
                return;
            }

            if ((uncaught && logLevel < this.autoReportLogLevel)) {
                Logger<IBugly>.L("Not report exception for level " + logLevel.ToString());
                return;
            }

            string type = null;
            string reason = null;

            if (!string.IsNullOrEmpty(message)) {
                try {
                    if ((LogSeverity.LogException == logLevel) && message.Contains("Exception")) {
                        Match match = new Regex(@"^(?<errorType>\S+):\s*(?<errorMessage>.*)", RegexOptions.Singleline).Match(message);

                        if (match.Success) {
                            type = match.Groups["errorType"].Value.Trim();
                            reason = match.Groups["errorMessage"].Value.Trim();
                        }
                    } else if ((LogSeverity.LogError == logLevel) && message.StartsWith("Unhandled Exception:")) {
                        Match match = new Regex(@"^Unhandled\s+Exception:\s*(?<exceptionName>\S+):\s*(?<exceptionDetail>.*)", RegexOptions.Singleline).Match(message);

                        if (match.Success) {
                            string exceptionName = match.Groups["exceptionName"].Value.Trim();
                            string exceptionDetail = match.Groups["exceptionDetail"].Value.Trim();

                            // 
                            int dotLocation = exceptionName.LastIndexOf(".");
                            if (dotLocation > 0 && dotLocation != exceptionName.Length) {
                                type = exceptionName.Substring(dotLocation + 1);
                            } else {
                                type = exceptionName;
                            }

                            int stackLocation = exceptionDetail.IndexOf(" at ");
                            if (stackLocation > 0) {
                                // 
                                reason = exceptionDetail.Substring(0, stackLocation);
                                // substring after " at "
                                string callStacks = exceptionDetail.Substring(stackLocation + 3).Replace(" at ", "\n").Replace("in <filename unknown>:0", "").Replace("[0x00000]", "");
                                //
                                stackTrace = string.Format("{0}\n{1}", stackTrace, callStacks.Trim());

                            } else {
                                reason = exceptionDetail;
                            }

                            // for LuaScriptException
                            if (type.Equals("LuaScriptException") && exceptionDetail.Contains(".lua") && exceptionDetail.Contains("stack traceback:")) {
                                stackLocation = exceptionDetail.IndexOf("stack traceback:");
                                if (stackLocation > 0) {
                                    reason = exceptionDetail.Substring(0, stackLocation);
                                    // substring after "stack traceback:"
                                    string callStacks = exceptionDetail.Substring(stackLocation + 16).Replace(" [", " \n[");

                                    //
                                    stackTrace = string.Format("{0}\n{1}", stackTrace, callStacks.Trim());
                                }
                            }
                        }
                    }
                } catch { }

                if (string.IsNullOrEmpty(reason)) {
                    reason = message;
                }
            }

            if (string.IsNullOrEmpty(name)) {
                if (string.IsNullOrEmpty(type)) {
                    type = string.Format("Unity{0}", logLevel.ToString());
                }
            } else {
                type = name;
            }

            _ReportException(uncaught, type, reason, stackTrace);
        }

        private void _HandleException(System.Exception e, string message, bool uncaught)
        {
            if (e == null) {
                return;
            }

            string name = e.GetType().Name;
            string reason = e.Message;

            if (!string.IsNullOrEmpty(message)) {
                reason = string.Format("{0}{1}***{2}", reason, Environment.NewLine, message);
            }

            StringBuilder stackTraceBuilder = new StringBuilder("");

            StackTrace stackTrace = new StackTrace(e, true);
            int count = stackTrace.FrameCount;
            for (int i = 0; i < count; ++i) {
                StackFrame frame = stackTrace.GetFrame(i);

                stackTraceBuilder.AppendFormat("{0}.{1}", frame.GetMethod().DeclaringType.Name, frame.GetMethod().Name);

                ParameterInfo[] parameters = frame.GetMethod().GetParameters();
                if (parameters == null || parameters.Length == 0) {
                    stackTraceBuilder.Append(" () ");
                } else {
                    stackTraceBuilder.Append(" (");

                    int pcount = parameters.Length;

                    ParameterInfo param = null;
                    for (int p = 0; p < pcount; p++) {
                        param = parameters[p];
                        stackTraceBuilder.AppendFormat("{0} {1}", param.ParameterType.Name, param.Name);

                        if (p != pcount - 1) {
                            stackTraceBuilder.Append(", ");
                        }
                    }
                    param = null;

                    stackTraceBuilder.Append(") ");
                }

                string fileName = frame.GetFileName();
                if (!string.IsNullOrEmpty(fileName) && !fileName.ToLower().Equals("unknown")) {
                    fileName = fileName.Replace("\\", "/");

                    int loc = fileName.ToLower().IndexOf("/assets/");
                    if (loc < 0) {
                        loc = fileName.ToLower().IndexOf("assets/");
                    }

                    if (loc > 0) {
                        fileName = fileName.Substring(loc);
                    }

                    stackTraceBuilder.AppendFormat("(at {0}:{1})", fileName, frame.GetFileLineNumber());
                }
                stackTraceBuilder.AppendLine();
            }

            // report
            _ReportException(uncaught, name, reason, stackTraceBuilder.ToString());
        }

        private void _ReportException(bool uncaught, string name, string reason, string stackTrace)
        {
            if (string.IsNullOrEmpty(name)) {
                return;
            }

            if (string.IsNullOrEmpty(stackTrace)) {
                stackTrace = StackTraceUtility.ExtractStackTrace();
            }

            if (string.IsNullOrEmpty(stackTrace)) {
                stackTrace = "Empty";
            } else {
                try {
                    string[] frames = stackTrace.Split('\n');

                    if (frames != null && frames.Length > 0) {

                        StringBuilder trimFrameBuilder = new StringBuilder();

                        string frame = null;
                        int count = frames.Length;
                        for (int i = 0; i < count; i++) {
                            frame = frames[i];

                            if (string.IsNullOrEmpty(frame) || string.IsNullOrEmpty(frame.Trim())) {
                                continue;
                            }

                            frame = frame.Trim();

                            // System.Collections.Generic
                            if (frame.StartsWith("System.Collections.Generic.") || frame.StartsWith("ShimEnumerator")) {
                                continue;
                            }
                            if (frame.StartsWith("Bugly")) {
                                continue;
                            }
                            if (frame.Contains("..ctor")) {
                                continue;
                            }

                            int start = frame.ToLower().IndexOf("(at");
                            int end = frame.ToLower().IndexOf("/assets/");

                            if (start > 0 && end > 0) {
                                trimFrameBuilder.AppendFormat("{0}(at {1}", frame.Substring(0, start).Replace(":", "."), frame.Substring(end));
                            } else {
                                trimFrameBuilder.Append(frame.Replace(":", "."));
                            }

                            trimFrameBuilder.AppendLine();
                        }

                        stackTrace = trimFrameBuilder.ToString();
                    }
                } catch {
                    Logger<IBugly>.W("Error to parse the stack trace");
                }

            }

            Logger<IBugly>.E(string.Format("ReportException: {0} {1}\n*********\n{2}\n*********", name, reason, stackTrace));

            this.uncaughtAutoReportOnce = uncaught && this.autoQuitApplicationAfterReport;

            this.proxy.Call("postException", TYPE_U3D_CRASH, name, reason, stackTrace, this.uncaughtAutoReportOnce);
        }

        private void _ConfigCrashReporter()
        {
            if (!this.crashReporterConfiged) {
                try {
                    switch (Application.platform) {
                    case RuntimePlatform.IPhonePlayer:
                        break;
                    case RuntimePlatform.Android:
                        this.proxy.Call("setSdkPackageName", "com.tencent.bugly");
                        break;
                    }
                    this.crashReporterConfiged = true;
                } catch { }
            }
        }

        private INativeProxy proxy = null;
        private event LogCallbackDelegate logCallbackEventHandler;
        private bool uncaughtAutoReportOnce = false;
        private LogSeverity autoReportLogLevel = LogSeverity.LogError;
        private bool autoQuitApplicationAfterReport = false;
        private bool crashReporterConfiged = false;

        private static readonly int TYPE_U3D_CRASH = 4;
        private static readonly int GAME_TYPE_UNITY = 2;
    }
}