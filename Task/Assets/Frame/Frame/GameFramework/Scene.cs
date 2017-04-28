// -----------------------------------------------------------------
// File:    Scene.cs
// Author:  mouguangyi
// Date:    2016.07.18
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public Scene()
        {
            this.dispatcher = new Dispatcher(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            this.dispatcher.Notify(Message.Destroy);
            this.dispatcher.Dispose();
            this.dispatcher = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public virtual void Enter(IGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public virtual void Exit(IGame game)
        {
            this.game = null;
            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        public virtual void Update(float delta)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void AddListener(string type, Action<object, Message> handler)
        {
            this.dispatcher.AddListener(type, handler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void RemoveListener(string type, Action<object, Message> handler)
        {
            this.dispatcher.RemoveListener(type, handler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="resetHandlersAfterSend"></param>
        public void Notify(Message message, bool resetHandlersAfterSend = false)
        {
            this.dispatcher.Notify(message, resetHandlersAfterSend);
        }

        /// <summary>
        /// 
        /// </summary>
        public IGame Game
        {
            get {
                return this.game;
            }
        }

        private IGame game = null;
        private Dispatcher dispatcher = null;
    }
}