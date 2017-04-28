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
    /// @details Log�����͡�
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// ��ʹ��Log��
        /// </summary>
        DISABLE = 0x0,

        /// <summary>
        /// ����Console���������
        /// </summary>
        CONSOLE = 0x1,

        /// <summary>
        /// ������Log�ļ���
        /// </summary>
        FILE = 0x2,

        /// <summary>
        /// ͬʱʹ��Console���ں�Log�ļ���
        /// </summary>
        MIXED = 0x3,
    }

    /// <summary>
    /// @details Log�ȼ���
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// �쳣�ȼ����ϱ���¼��
        /// </summary>
        EXCEPTION = 0,

        /// <summary>
        /// ����ȼ����ϱ���¼��
        /// </summary>
        ERROR = 1,

        /// <summary>
        /// ����ȼ����ϱ���¼��
        /// </summary>
        WARNING = 2,

        /// <summary>
        /// ����Log����¼��
        /// </summary>
        VERBOSE = 3,
    }

    /// <summary>
    /// @details ��־���������ࡣ
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="name">��־���ơ�</param>
        /// <param name="type">��־���͡�<see cref="LogType"/></param>
        /// <param name="level">��־�ȼ���<see cref="LogLevel"/></param>
        /// <param name="enabled">�Ƿ�ʹ�ø���־��</param>
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
        /// �Ƿ�ʹ����־��
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
    /// @details ����Service���ʹ�����Logger����ÿһ��Service��Logger���ļ����������»�
    /// ����һ����Service��NamespaceΪ�ļ�������־�ļ�����Service��ģ������������
    /// ������Service�����к��Ѿ���һ��Loggerȫ�ֵ������󱻴���������ֱ��ʹ�á�
    /// </summary>
    /// <typeparam name="T">�̳���IService�Ľӿ����͡�</typeparam>
    public sealed class Logger<T> : Logger where T : IService
    {
        /// <summary>
        /// Logger���캯������Ҫͨ��new��������Ӧ��Service��Logger����
        /// </summary>
        /// <param name="type">��־���͡�<see cref="LogType"/></param>
        /// <param name="level">��־�ȼ���<see cref="LogLevel"/></param>
        /// <param name="enabled">�Ƿ�ʹ�ø���־��</param>
        public Logger(LogType type, LogLevel level, bool enabled) : base(typeof(T).Namespace, type, level, enabled)
        {
            Logger<T>.instance = this;
        }

        /// <summary>
        /// ��ͨ��־��
        /// </summary>
        /// <param name="message"></param>
        public static void L(object message)
        {
            if (null != Logger<T>.instance) {
                Logger<T>.instance.Log(message);
            }
        }

        /// <summary>
        /// ���档
        /// </summary>
        /// <param name="message"></param>
        public static void W(object message)
        {
            if (null != Logger<T>.instance) {
                Logger<T>.instance.Warning(message);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="message"></param>
        public static void E(object message)
        {
            if (null != Logger<T>.instance) {
                Logger<T>.instance.Error(message);
            }
        }

        /// <summary>
        /// �쳣��
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
    /// @details ȫ��Logger������Service�ⲿ��Log������ʹ�ø�ȫ��Logger����¼��
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
        /// ��ͨ��־��
        /// </summary>
        /// <param name="message"></param>
        public static void L(object message)
        {
            _GetInstance().Log(message);
        }

        /// <summary>
        /// ���档
        /// </summary>
        /// <param name="message"></param>
        public static void W(object message)
        {
            _GetInstance().Warning(message);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="message"></param>
        public static void E(object message)
        {
            _GetInstance().Error(message);
        }

        /// <summary>
        /// �쳣��
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