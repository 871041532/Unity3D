// -----------------------------------------------------------------
// File:    Logger.cs
// Author:  mouguangyi
// Date:    2016.06.29
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// @details Log的类型。
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 不使用Log。
        /// </summary>
        DISABLE = 0x0,

        /// <summary>
        /// 仅在Console窗口输出。
        /// </summary>
        CONSOLE = 0x1,

        /// <summary>
        /// 仅存入Log文件。
        /// </summary>
        FILE = 0x2,

        /// <summary>
        /// 同时使用Console窗口和Log文件。
        /// </summary>
        MIXED = 0x3,
    }

    /// <summary>
    /// @details Log等级。
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 异常等级以上被记录。
        /// </summary>
        EXCEPTION = 0,

        /// <summary>
        /// 错误等级以上被记录。
        /// </summary>
        ERROR = 1,

        /// <summary>
        /// 警告等级以上被记录。
        /// </summary>
        WARNING = 2,

        /// <summary>
        /// 所有Log都记录。
        /// </summary>
        VERBOSE = 3,
    }

    /// <summary>
    /// @details 日志基础抽象类。
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="name">日志名称。</param>
        /// <param name="type">日志类型。<see cref="LogType"/></param>
        /// <param name="level">日志等级。<see cref="LogLevel"/></param>
        /// <param name="enabled">是否使用该日志。</param>
        public Logger(string name, LogType type, LogLevel level, bool enabled)
        {
            this.name = name;
            this.type = type;
            this.level = level;
            this.enabled = enabled;
        }

        internal virtual void _Init()
        { }

        internal virtual void _Dispose()
        {
            if (_IsFileEnabled() && this.fileMessages.Count > 0) {
                using (var writer = File.AppendText(PathUtility.ComposeDataPath("/" + this.name + ".log"))) {
                    writer.WriteLine("================================================================");
                    for (var i = 0; i < this.fileMessages.Count; ++i) {
                        writer.WriteLine(this.fileMessages[i]);
                    }
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected void Log(object message)
        {
            if (_IsVerboseLevel()) {
                if (_IsConsoleEnabled()) {
                    UnityEngine.Debug.Log("[" + this.name + "] " + message);
                }
                if (_IsFileEnabled()) {
                    this.fileMessages.Add(_GetDateTime() + "-[L] " + message);
                }
            }

            Console.L(this.name, "" + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected void Warning(object message)
        {
            if (_IsWarningLevel()) {
                if (_IsConsoleEnabled()) {
                    UnityEngine.Debug.LogWarning("[" + this.name + "] " + message);
                }
                if (_IsFileEnabled()) {
                    this.fileMessages.Add(_GetDateTime() + "-[W] " + message);
                }
            }

            Console.W(this.name, "" + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected void Error(object message)
        {
            if (_IsErrorLevel()) {
                if (_IsConsoleEnabled()) {
                    UnityEngine.Debug.LogError("[" + this.name + "] " + message);
                }
                if (_IsFileEnabled()) {
                    this.fileMessages.Add(_GetDateTime() + "-[E] " + message);
                }
            }

            Console.E(this.name, "" + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        protected void Exception(Exception exception)
        {
            if (_IsExceptionLevel()) {
                if (_IsConsoleEnabled()) {
                    UnityEngine.Debug.LogException(new Exception("[" + this.name + "] " + exception.Message));
                }
                if (_IsFileEnabled()) {
                    this.fileMessages.Add(_GetDateTime() + "-[X] " + exception.Message);
                }
            }

            Console.X(this.name, exception.Message);
        }

        /// <summary>
        /// 是否使用日志。
        /// </summary>
        protected bool Enabled
        {
            get {
                return this.enabled && this.type != LogType.DISABLE;
            }
        }

        private bool _IsConsoleEnabled()
        {
            return (this.enabled && 0 != (this.type & LogType.CONSOLE));
        }

        private bool _IsFileEnabled()
        {
            return (this.enabled && 0 != (this.type & LogType.FILE));
        }

        private bool _IsVerboseLevel()
        {
            return (this.level >= LogLevel.VERBOSE);
        }

        private bool _IsWarningLevel()
        {
            return (this.level >= LogLevel.WARNING);
        }

        private bool _IsErrorLevel()
        {
            return (this.level >= LogLevel.ERROR);
        }

        private bool _IsExceptionLevel()
        {
            return (this.level >= LogLevel.EXCEPTION);
        }

        private string _GetDateTime()
        {
            return DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string name = "";
        private LogType type = LogType.DISABLE;
        private LogLevel level = LogLevel.VERBOSE;
        private bool enabled = true;
        private List<string> fileMessages = new List<string>();
    }

    /// <summary>
    /// @details 根据Service类型创建的Logger对象。每一个Service的Logger在文件输出的情况下会
    /// 生成一个以Service的Namespace为文件名的日志文件。在Service的模块中无须自行
    /// 创建，Service在运行后已经有一个Logger全局单例对象被创建，可以直接使用。
    /// </summary>
    /// <typeparam name="T">继承自IService的接口类型。</typeparam>
    public sealed class Logger<T> : Logger where T : IService
    {
        /// <summary>
        /// Logger构造函数，不要通过new来创建对应的Service的Logger对象。
        /// </summary>
        /// <param name="type">日志类型。<see cref="LogType"/></param>
        /// <param name="level">日志等级。<see cref="LogLevel"/></param>
        /// <param name="enabled">是否使用该日志。</param>
        public Logger(LogType type, LogLevel level, bool enabled) : base(typeof(T).Namespace, type, level, enabled)
        {
            Logger<T>.instance = this;
        }

        /// <summary>
        /// 普通日志。
        /// </summary>
        /// <param name="message"></param>
        public static void L(object message)
        {
            if (null != Logger<T>.instance) {
                Logger<T>.instance.Log(message);
            }
        }

        /// <summary>
        /// 警告。
        /// </summary>
        /// <param name="message"></param>
        public static void W(object message)
        {
            if (null != Logger<T>.instance) {
                Logger<T>.instance.Warning(message);
            }
        }

        /// <summary>
        /// 错误。
        /// </summary>
        /// <param name="message"></param>
        public static void E(object message)
        {
            if (null != Logger<T>.instance) {
                Logger<T>.instance.Error(message);
            }
        }

        /// <summary>
        /// 异常。
        /// </summary>
        /// <param name="exception"></param>
        public static void X(Exception exception)
        {
            if (null != Logger<T>.instance) {
                Logger<T>.instance.Exception(exception);
            }
        }

        internal override void _Dispose()
        {
            Logger<T>.instance = null;

            base._Dispose();
        }

        private static Logger<T> instance = null;
    }

    /// <summary>
    /// @details 全局Logger，所有Service外部的Log都可以使用该全局Logger来记录。
    /// </summary>
    public sealed class AnyLogger : Logger
    {
        private AnyLogger(LogType type, LogLevel level) : base(Application.productName, type, level, true)
        { }

        internal override void _Init()
        {
            base._Init();

            Application.logMessageReceived += _OnLogMessageReceived;
        }

        internal override void _Dispose()
        {
            Application.logMessageReceived -= _OnLogMessageReceived;

            AnyLogger.instance = null;
            base._Dispose();
        }

        private void _OnLogMessageReceived(string condition, string stackTrace, UnityEngine.LogType type)
        {
            if (UnityEngine.LogType.Exception == type) {
                // TODO: Collection log info and send to remote server???
            }
        }

        /// <summary>
        /// 普通日志。
        /// </summary>
        /// <param name="message"></param>
        public static void L(object message)
        {
            _GetInstance().Log(message);
        }

        /// <summary>
        /// 警告。
        /// </summary>
        /// <param name="message"></param>
        public static void W(object message)
        {
            _GetInstance().Warning(message);
        }

        /// <summary>
        /// 错误。
        /// </summary>
        /// <param name="message"></param>
        public static void E(object message)
        {
            _GetInstance().Error(message);
        }

        /// <summary>
        /// 异常。
        /// </summary>
        /// <param name="exception"></param>
        public static void X(Exception exception)
        {
            _GetInstance().Exception(exception);
        }

        internal static AnyLogger _GetInstance()
        {
            if (null == AnyLogger.instance) {
                var center = ServiceCenter.Center();
                AnyLogger.instance = new AnyLogger(center._LogType, center._LogLevel);
            }

            return AnyLogger.instance;
        }

        private static AnyLogger instance = null;
    }
}