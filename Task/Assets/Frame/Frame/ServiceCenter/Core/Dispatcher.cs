// -----------------------------------------------------------------
// File:    Dispatcher.cs
// Author:  mouguangyi
// Date:    2016.06.15
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameBox.Framework
{
    /// <summary>
    /// @details 消息分发器。
    /// </summary>
    public class Dispatcher : C0
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="target">发送的主体。</param>
        public Dispatcher(object target)
        {
            this.target = target;
        }

        /// <summary>
        /// 析构函数。
        /// </summary>
        public override void Dispose()
        {
            if (null != this.listeners) {
                this.listeners.Clear();
                this.listeners = null;
            }

            base.Dispose();
        }

        /// <summary>
        /// 添加监听函数句柄。
        /// </summary>
        /// <param name="type">监听的消息类型。</param>
        /// <param name="handler">函数句柄。</param>
        public void AddListener(string type, Action<object, Message> handler)
        {
            if (null == handler) {
                return;
            }

            List<Action<object, Message>> handlers = null;
            if (this.listeners.ContainsKey(type)) {
                handlers = this.listeners[type];
            } else {
                handlers = new List<Action<object, Message>>();
                this.listeners.Add(type, handlers);
            }

            var index = handlers.IndexOf(handler);
            if (index < 0) {
                handlers.Add(handler);
            }
        }

        /// <summary>
        /// 删除监听函数句柄。
        /// </summary>
        /// <param name="type">监听的消息类型。</param>
        /// <param name="handler">函数句柄。</param>
        public void RemoveListener(string type, Action<object, Message> handler)
        {
            if (null == handler) {
                return;
            }

            if (this.listeners.ContainsKey(type)) {
                var handlers = this.listeners[type];
                handlers.Remove(handler);
            }
        }

        /// <summary>
        /// 广播消息。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="resetHandlersAfterSend">是否在发送后清空监听的函数句柄。</param>
        public void Notify(Message message, bool resetHandlersAfterSend = false)
        {
            if (Message.ANY == message.Type) {
                foreach (var pair in this.listeners) {
                    _NotifyHandlers(pair.Value, message);
                }

                if (resetHandlersAfterSend) {
                    _ResetAll();
                }
            } else {
                List<Action<object, Message>> handlers = null;
                if (this.listeners.TryGetValue(message.Type, out handlers)) {
                    _NotifyHandlers(handlers, message);

                    if (resetHandlersAfterSend) {
                        _Reset(message.Type);
                    }
                }
                if (this.listeners.TryGetValue(Message.ANY, out handlers)) {
                    _NotifyHandlers(handlers, message);
                }
            }
        }

        private void _Reset(string type)
        {
            if (null != this.listeners) {
                List<Action<object, Message>> handlers = null;
                if (this.listeners.TryGetValue(type, out handlers)) {
                    handlers.Clear();
                }
            }
        }

        private void _ResetAll()
        {
            if (null != this.listeners) {
                this.listeners.Clear();
            }
        }

        private void _NotifyHandlers(List<Action<object, Message>> handlers, Message message)
        {
            var count = handlers.Count;
            for (var i = count - 1; i >= 0; --i) {
                handlers[i](this.target, message);
            }
        }

        private object target = null;
        private Dictionary<string, List<Action<object, Message>>> listeners = new Dictionary<string, List<Action<object, Message>>>();

        /// <summary>
        /// 全局消息分发器。
        /// </summary>
        public static Dispatcher Global
        {
            get {
                if (null == Dispatcher.globalInstance) {
                    Dispatcher.globalInstance = new Dispatcher(null);
                }

                return Dispatcher.globalInstance;
            }
        }

        private static Dispatcher globalInstance = null;
    }
}