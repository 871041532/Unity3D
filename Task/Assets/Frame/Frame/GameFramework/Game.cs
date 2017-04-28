// -----------------------------------------------------------------
// File:    Game.cs
// Author:  mouguangyi
// Date:    2016.07.18
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GameBox.Service.GameFramework
{
    class Game : IGame, IServiceGraph
    {
        public Game()
        { }

        public string Id
        {
            get {
                return "com.giant.service.gameframework";
            }
        }

        public virtual void Run(IServiceRunner runner)
        {
           // runner.Ready();
        }

        public void Pulse(float delta)
        {
            if (null != this.scene) {
                this.scene.Update(delta);
            }
        }

        public void Terminate()
        {
            if (null != this.scene) {
                this.scene.Exit(this);
            }
        }

        public void GotoScene(Scene scene)
        {
            if (null != this.scene) {
                this.scene.Exit(this);
            }

            this.scene = scene;

            if (null != this.scene) {
                this.scene.Enter(this);
            }
        }

        public void SetUserData<T>(string key, T data)
        {
            this.userDatas.Add(key, data);
        }

        public T GetUserData<T>(string key)
        {
            object data = null;
            if (this.userDatas.TryGetValue(key, out data)) {
                return (T)data;
            } else {
                return default(T);
            }
        }

        protected Scene Scene
        {
            get {
                return this.scene;
            }
        }

        private Scene scene = null;
        private Dictionary<string, object> userDatas = new Dictionary<string, object>();

        // ---------------------------------------------------------
        // Graph
        public virtual void Draw()
        {
            GUI.DrawTexture(new Rect(0, 0, GraphStyle.ServiceWidth, SCENE_HEIGHT), GraphStyle.GreenTexture);
            GUI.Label(new Rect(0, 0, GraphStyle.ServiceWidth, SCENE_HEIGHT), this.scene.GetType().Name, GraphStyle.MiddleLabel);
        }

        public virtual float Width
        {
            get {
                return GraphStyle.ServiceWidth;
            }
        }

        public virtual float Height
        {
            get {
                return SCENE_HEIGHT;
            }
        }

        private const float SCENE_HEIGHT = 30f;
    }
}