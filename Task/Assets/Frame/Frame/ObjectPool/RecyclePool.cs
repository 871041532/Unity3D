// -----------------------------------------------------------------
// File:    RecyclePool.cs
// Author:  mouguangyi
// Date:    2016.12.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameBox.Service.ObjectPool
{
    sealed class RecyclePool : C0, IRecyclePool
    {
        public RecyclePool(IRecycleProcesser processer, IRecycleFactory factory)
        {
            this.processer = processer;
            this.factory = factory;
        }

        public override void Dispose()
        {
            foreach (var jar in this.pool.Values) {
                _DisposeObjectJar(jar);
            }
            this.pool = null;

            base.Dispose();
        }

        #region IRecyclePool
        public void Preload(string objectType, int count)
        {
            if (null != this.factory) {
                ObjectJar jar = _GetObjectJar(objectType);
                for (var i = 0; i < count; ++i) {
                    var recycleObject = this.factory.CreateObject(objectType);
                    this.processer.ReclaimObject(recycleObject);
                    jar.Drop(recycleObject);
                }
            }
        }

        public void PreloadAsync(string objectType, int count, Action callback)
        {
            if (null != this.factory) {
                ObjectJar jar = _GetObjectJar(objectType);
                AsyncTask task = new CompletedTask();
                for (var i = 0; i < count; ++i) {
                    task = task.Continue(t =>
                    {
                        return new CreateObjectAsyncTask(jar, objectType, this.processer, this.factory);
                    });
                }

                task.Start().Continue(t =>
                {
                    if (null != callback) {
                        callback();
                    }
                    return null;
                });
            }
        }

        public T Pick<T>(string objectType)
        {
            ObjectJar jar = _GetObjectJar(objectType);
            var recycleObject = jar.Pick();
            if (null == recycleObject && null != this.factory) {
                recycleObject = this.factory.CreateObject(objectType);

                // Load more 1/2 jar MaxCount objects
                if (!this.createOperations.Contains(objectType)) {
                    lock (this.createOperations) {
                        this.createOperations.Add(objectType);
                    }
                    PreloadAsync(objectType, jar.MaxCount >> 1, () =>
                    {
                        lock (this.createOperations) {
                            this.createOperations.Remove(objectType);
                        }
                    });
                }
            }

            if (null != recycleObject) {
                this.processer.RecoverObject(recycleObject);
            }

            return (T)recycleObject;
        }

        public void Drop(string objectType, object recycleObject)
        {
            if (null != recycleObject) {
                this.processer.ReclaimObject(recycleObject);
                ObjectJar jar = _GetObjectJar(objectType);
                jar.Drop(recycleObject);
            }
        }
        #endregion

        private void _DisposeObjectJar(ObjectJar jar)
        {
            while (jar.Count > 0) {
                object recycleObject = jar.Pick();
                if (null != recycleObject) {
                    this.factory.DestroyObject(recycleObject);
                }
            }
            jar.Dispose();
        }

        private ObjectJar _GetObjectJar(string type)
        {
            ObjectJar jar = null;
            if (!this.pool.TryGetValue(type, out jar)) {
                this.pool.Add(type, jar = new ObjectJar());
            }

            return jar;
        }

        private object _Pick(string type)
        {
            ObjectJar jar = _GetObjectJar(type);
            var recycleObject = jar.Pick();
            if (null == recycleObject) {
                recycleObject = this.factory.CreateObject(type);
            }

            if (null != recycleObject) {
                this.processer.RecoverObject(recycleObject);
            }

            return recycleObject;
        }

        private class CreateObjectAsyncTask : AsyncTask
        {
            public CreateObjectAsyncTask(ObjectJar jar, string type, IRecycleProcesser processer, IRecycleFactory factory) : base(false)
            {
                factory.CreateObjectAsync(type, obj =>
                {
                    processer.ReclaimObject(obj);
                    jar.Drop(obj);
                    this.completed = true;
                });
            }

            protected override bool IsDone()
            {
                return this.completed;
            }

            private bool completed = false;
        }

        private IRecycleProcesser processer = null;
        private IRecycleFactory factory = null;
        private Dictionary<string, ObjectJar> pool = new Dictionary<string, ObjectJar>();
        private HashSet<string> createOperations = new HashSet<string>();

        // - Graph
        public float DrawGraph(float yOffset)
        {
            foreach (var item in this.pool) {
                GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, CHUNK_GRAPH_HEIGHT), GraphStyle.GreenTexture);
                GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, CHUNK_GRAPH_HEIGHT), item.Key + ":" + item.Value.Count + "/" + item.Value.MaxCount, GraphStyle.MiniLabel);
                yOffset += CHUNK_GRAPH_HEIGHT;
            }

            return yOffset;
        }

        public float GraphHeight
        {
            get {
                float height = 0f;
                foreach (var item in this.pool) {
                    height += CHUNK_GRAPH_HEIGHT;
                }

                return height;
            }
        }

        private const float CHUNK_GRAPH_HEIGHT = 20f;
    }
}