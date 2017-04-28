// -----------------------------------------------------------------
// File:    RecycleManager.cs
// Author:  mouguangyi
// Date:    2017.03.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System.Collections.Generic;

namespace GameBox.Service.ObjectPool
{
    sealed class RecycleManager : IRecycleManager, IServiceGraph
    {
        #region IService
        public string Id
        {
            get {
                return "com.giant.service.objectpool";
            }
        }

        public void Run(IServiceRunner runner)
        {
            runner.Ready(_Terminate);
        }

        public void Pulse(float delta)
        { }
        #endregion

        #region IPoolManager
        public IRecyclePool Create(string poolType, IRecycleProcesser processer, IRecycleFactory factory = null)
        {
            RecyclePool pool = null;
            if (!this.pools.TryGetValue(poolType, out pool)) {
                this.pools.Add(poolType, pool = new RecyclePool(processer, factory));
            }

            return pool;
        }

        public IRecyclePool Find(string poolType)
        {
            RecyclePool pool = null;
            if (this.pools.TryGetValue(poolType, out pool)) {
                return pool;
            } else {
                return null;
            }
        }
        #endregion

        private void _Terminate()
        {
            foreach (var pool in this.pools.Values) {
                pool.Dispose();
            }
            this.pools = null;
        }

        private Dictionary<string, RecyclePool> pools = new Dictionary<string, RecyclePool>();

        // - IServiceGraph
        public void Draw()
        {
            float yOffset = 0f;
            foreach (var pool in this.pools.Values) {
                yOffset = pool.DrawGraph(yOffset);
            }
        }

        public float Width
        {
            get {
                return GraphStyle.ServiceWidth;
            }
        }

        public float Height
        {
            get {
                float height = 0f;
                foreach (var pool in this.pools.Values) {
                    height += pool.GraphHeight;
                }

                return height;
            }
        }
    }
}