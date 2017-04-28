// -----------------------------------------------------------------
// File:    SceneObject.cs
// Author:  mouguangyi
// Date:    2016.11.15
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    enum SceneObjectState
    {
        UNLOADED = -1,
        LOADED = 1,
    }

    class SceneObject : C0
    {
        public SceneObject(SceneObjectData objectData, GameObject objectRoot)
        {
            this.objectData = objectData;
            this.objectRoot = objectRoot;
            this.objectRoot.transform.localPosition = new Vector3(objectData.PositionX, objectData.PositionY, objectData.PositionZ);
            this.objectRoot.transform.localRotation = new Quaternion(objectData.OrientationX, objectData.OrientationY, objectData.OrientationZ, objectData.OrientationW);
            this.objectRoot.transform.localScale = new Vector3(objectData.ScaleX, objectData.ScaleY, objectData.ScaleZ);
        }

        public virtual void Load()
        { }

        public virtual void LoadAsync(Action handler)
        { }

        public virtual void Unload()
        {
            this.loaded = false;
        }

        public virtual void OnEnter()
        { }

        public virtual void OnExit()
        { }

        public virtual void OnUpdate(float delta)
        { }

        public virtual void OnTrigger()
        { }

        public Vector3 Position
        {
            get {
                return this.position;
            }
        }

        public Quaternion Orientation
        {
            get {
                return this.orientation;
            }
        }

        protected void NotifyLoaded(Action handler)
        {
            this.loaded = true;
            new CompletedTask().Start().Continue(task =>
            {
                if (null != handler) {
                    handler();
                }

                return null;
            });
        }

        protected SceneObjectData ObjectData
        {
            get {
                return this.objectData;
            }
        }

        protected GameObject ObjectRoot
        {
            get {
                return this.objectRoot;
            }
        }

        internal bool _Loaded
        {
            get {
                return this.loaded;
            }
        }

        private SceneObjectData objectData = null;
        private GameObject objectRoot = null;
        private Vector3 position = new Vector3();
        private Quaternion orientation = new Quaternion();
        private bool loaded = false;

        // --
        internal virtual float _DrawGraph(float yOffset)
        {
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, OBJECT_GRAPH_HEIGHT), GraphStyle.RedTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, OBJECT_GRAPH_HEIGHT), this.objectData.Name, GraphStyle.MiniLabel);

            return yOffset + OBJECT_GRAPH_HEIGHT;
        }

        internal virtual float _GraphHeight
        {
            get {
                return OBJECT_GRAPH_HEIGHT;
            }
        }

        protected const float OBJECT_GRAPH_HEIGHT = 20f;
    }
}