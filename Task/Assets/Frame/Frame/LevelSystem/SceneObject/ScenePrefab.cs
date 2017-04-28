// -----------------------------------------------------------------
// File:    ScenePrefab.cs
// Author:  mouguangyi
// Date:    2016.11.15
// Description:
//      
// -----------------------------------------------------------------
using System;
using GameBox.Framework;
using GameBox.Service.AssetManager;
using UnityEngine;
using GameBox.Service.ObjectPool;

namespace GameBox.Service.LevelSystem
{
    class ScenePrefab : SceneObject
    {
        public ScenePrefab(SceneObjectData objectData, GameObject objectRoot) : base(objectData, objectRoot)
        {
            if (SceneLevel.IsInEdit()) {
                var prefabObject = SceneLevel.GetOrAddComponent<PrefabObject>(this.ObjectRoot);
                prefabObject.PrefabPath = (this.ObjectData as ScenePrefabData).Path;
            }
        }

        public override void Dispose()
        {
            if (null != this.asset) {
                this.asset.Dispose();
                this.asset = null;
            }

            if (null != this.recycleObject) {
                GameObject.Destroy(this.recycleObject);
                this.recycleObject = null;
            }

            base.Dispose();
        }

        public override void Load()
        {
            var prefabData = this.ObjectData as ScenePrefabData;
            this.recycleObject = _GetRecyclePool().Pick<GameObject>(prefabData.Path);
            if (null == this.recycleObject) {
                this.asset = ServiceCenter.GetService<IAssetManager>().Load(prefabData.Path, AssetType.PREFAB);
                this.recycleObject = GameObject.Instantiate(this.asset.Cast<GameObject>()) as GameObject;
            }
            _Init(this.recycleObject, prefabData);
        }

        public override void LoadAsync(Action handler)
        {
            var prefabData = this.ObjectData as ScenePrefabData;
            this.recycleObject = _GetRecyclePool().Pick<GameObject>(prefabData.Path);
            if (null != this.recycleObject) {
                _Init(this.recycleObject, prefabData);
                NotifyLoaded(handler);
            } else {
                ServiceCenter.GetService<IAssetManager>().LoadAsync(prefabData.Path, AssetType.PREFAB, asset =>
                {
                    this.asset = asset;
                    this.recycleObject = GameObject.Instantiate(this.asset.Cast<GameObject>()) as GameObject;

                    _Init(this.recycleObject, prefabData);
                    NotifyLoaded(handler);
                });
            }
        }

        public override void Unload()
        {
            if (null != this.asset) {
                this.asset.Dispose();
                this.asset = null;
            }

            if (null != this.recycleObject) {
                var prefabData = this.ObjectData as ScenePrefabData;
                _GetRecyclePool().Drop(prefabData.Path, this.recycleObject);
                this.recycleObject = null;
            }

            base.Unload();
        }

        private void _Init(GameObject go, ScenePrefabData prefabData)
        {
            go.transform.SetParent(this.ObjectRoot.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }

        private IRecyclePool _GetRecyclePool()
        {
            return ServiceCenter.GetService<IRecycleManager>().Find(OBJECTPOOLTYPE);
        }

        private class ScenePrefabProcesser : IRecycleProcesser
        {
            public void ReclaimObject(object recycleObject)
            {
                var go = recycleObject as GameObject;
                go.transform.position = new Vector3(int.MinValue, int.MinValue, int.MinValue);
            }

            public void RecoverObject(object recycleObject)
            { }
        }

        private IAsset asset = null;
        private GameObject recycleObject = null;

        public static void CreateRecyclePool()
        {
            ServiceCenter.GetService<IRecycleManager>().Create(OBJECTPOOLTYPE, new ScenePrefabProcesser());
        }

        private const string OBJECTPOOLTYPE = "GameBox.Service.LevelSystem.ScenePrefab";

        // -- Graph
        internal override float _DrawGraph(float yOffset)
        {
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, OBJECT_GRAPH_HEIGHT), GraphStyle.RedTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, OBJECT_GRAPH_HEIGHT), (this.ObjectData as ScenePrefabData).Path, GraphStyle.MiniLabel);

            return yOffset + OBJECT_GRAPH_HEIGHT;
        }
    }
}