// -----------------------------------------------------------------
// File:    GiantLightScene.cs
// Author:  mouguangyi
// Date:    2016.07.18
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.GiantLightFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class GiantLightScene : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public GiantLightScene()
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
        public virtual void Enter(IGiantGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public virtual void Exit(IGiantGame game)
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
        public virtual void Disconnect()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        public void SendRequest(uint id, string service, string method, byte[] content)
        {
            var game = this.game as GiantLightGame;
            game.SendRequest(id, service, method, content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        public void SendResponse(uint id, byte[] content)
        {
            var game = this.game as GiantLightGame;
            game.SendResponse(id, content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual bool PushRequest(uint id, string service, string method, byte[] content)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual bool PushResponse(uint id, byte[] content)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public IGiantGame Game
        {
            get {
                return this.game;
            }
        }

        private IGiantGame game = null;
        private Dispatcher dispatcher = null;
    }
}