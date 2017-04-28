// -----------------------------------------------------------------
// File:    SceneChunk.cs
// Author:  mouguangyi
// Date:    2016.11.15
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    class SceneChunk : C0
    {
        internal enum States
        {
            INVALID = -1,
            UNLOADED = 0,
            DATALOADING = 1,
            OBJECTLOADING = 2,
            LOADED = 3,
            UNLOADING = 4,
        }

        public SceneChunk(int index, SceneChunkRefData chunkRefData, GameObject chunkRoot, bool validateData)
        {
            this.index = index;
            this.chunkRefData = chunkRefData;
            this.chunkRoot = chunkRoot;
            this.chunkRoot.transform.localPosition = new Vector3(chunkRefData.PositionX, chunkRefData.PositionY, chunkRefData.PositionZ);
            this.chunkRoot.transform.localRotation = new Quaternion(chunkRefData.OrientationX, chunkRefData.OrientationY, chunkRefData.OrientationZ, chunkRefData.OrientationW);
            this.chunkRoot.transform.localScale = Vector3.one;
            this.validateData = validateData;
            
            this.dispatcher = new Dispatcher(this);

            this.stateSet = new StateSet<States>();

            // Data loading
            var dataLoadingState = new ChunkDataLoadingState(States.DATALOADING);
            dataLoadingState.UnloadStateId = States.UNLOADING;
            this.stateSet.AddState(dataLoadingState.StateId, dataLoadingState);

            // Object loading
            var objectLoadingState = new ChunkObjectLoadingState(States.OBJECTLOADING);
            objectLoadingState.UnloadStateId = States.UNLOADING;
            this.stateSet.AddState(objectLoadingState.StateId, objectLoadingState);

            // Loaded
            var loadedState = new ChunkLoadedState(States.LOADED);
            loadedState.LoadStateId = States.LOADED;
            loadedState.UnloadStateId = States.UNLOADING;
            this.stateSet.AddState(loadedState.StateId, loadedState);

            // Unloading
            var unloadingState = new ChunkUnloadingState(States.UNLOADING);
            this.stateSet.AddState(unloadingState.StateId, unloadingState);

            // Unloaded
            var unloadedState = new ChunkUnloadedState(States.UNLOADED);
            unloadedState.LoadStateId = States.DATALOADING;
            unloadedState.UnloadStateId = States.UNLOADED;
            this.stateSet.AddState(unloadedState.StateId, unloadedState);

            this.controller = new StateMachine(this);
            _ChangeState(States.UNLOADED);

            if (SceneLevel.IsInEdit()) {
                var chunkObject = SceneLevel.GetOrAddComponent<ChunkObject>(this.chunkRoot);
                chunkObject.Path = chunkRefData.Path;
            }
        }

        public override void Dispose()
        {
            _Clear();

            if (null != this.controller) {
                this.controller.Dispose();
            }

            GameObject.Destroy(this.chunkRoot);

            this.dispatcher.Dispose();

            this.dispatcher = null;
            this.controller = null;

            base.Dispose();
        }

        public void Load()
        {
            using (var asset = ServiceCenter.GetService<IAssetManager>().Load(this.chunkRefData.Path + ".bytes", AssetType.BYTES)) {
                var bytes = asset.Cast<byte[]>();
                if (null != bytes) {
                    this.objects = new List<SceneObject>();
                    using (var stream = new MemoryStream(bytes)) {
                        var chunkData = SceneChunkData.Deserialize(stream, this.validateData);
                        for (var i = 0; i < chunkData.Objects.Count; ++i) {
                            var go = new GameObject();
                            go.transform.SetParent(this.chunkRoot.transform);

                            var objectData = chunkData.Objects[i];
                            SceneObject obj = null;
                            switch (objectData.Type) {
                            case SceneObjectType.PREFAB:
                                obj = new ScenePrefab(objectData, go);
                                go.name = "_Prefab";
                                break;
                            case SceneObjectType.LIGHTMODIFIER:
                                obj = new SceneLightModifier(objectData, go);
                                go.name = "_LightModifier";
                                break;
                            default:
                                go.name = "_SceneObject";
                                break;
                            }
                            this.objects.Add(obj);
                            obj.Load();
                        }
                    }
                }
            }
        }

        public void LoadAsync(Action<object, Message> handler)
        {
            this.lifeTime = LIFE_TIME;
            this.dispatcher.AddListener(Message.COMPLETED, handler);

            var state = this.controller.State as ChunkState;
            if (States.INVALID != state.LoadStateId) {
                var loadState = this.stateSet.FindState(state.LoadStateId);
                this.controller.ChangeState(loadState);

                if (States.UNLOADED == state.StateId) {
                    state.LoadStateId = States.OBJECTLOADING;
                }
            }
        }

        public void UnloadAsync(Action<object, Message> handler)
        {
            this.dispatcher.AddListener(Message.DESTROY, handler);

            var state = this.controller.State as ChunkState;
            if (States.INVALID != state.UnloadStateId) {
                var unloadState = this.stateSet.FindState(state.UnloadStateId) as ChunkState;
                this.controller.ChangeState(unloadState);

                if (States.UNLOADING == state.UnloadStateId) {
                    unloadState.LoadStateId = state.StateId;
                }
            }
        }

        public void OnEnter()
        {
            for (var i = 0; i < this.objects.Count; ++i) {
                this.objects[i].OnEnter();
            }
        }

        public void OnExit()
        {
            for (var i = 0; i < this.objects.Count; ++i) {
                this.objects[i].OnExit();
            }
        }

        public void OnUpdate(float delta)
        {
            if (null != this.controller) {
                this.controller.Pulse(delta);
            }
        }

        public int Index
        {
            get {
                return this.index;
            }
        }

        internal void _ChangeState(SceneChunk.States stateId)
        {
            var state = this.stateSet.FindState(stateId);
            if (null != state) {
                this.controller.ChangeState(state);
            }
        }

        internal SceneChunkRefData _ChunkRefData
        {
            get {
                return this.chunkRefData;
            }
        }

        internal GameObject _ChunkRoot
        {
            get {
                return this.chunkRoot;
            }
        }

        internal bool _ValidateData
        {
            get {
                return this.validateData;
            }
        }

        internal List<SceneObject> _Objects
        {
            get {
                return this.objects;
            }
            set {
                this.objects = value;
            }
        }

        internal float _LifeTime
        {
            get {
                return this.lifeTime;
            }
            set {
                this.lifeTime = value;
            }
        }

        internal void _NotifyCompleted()
        {
            if (null != this.dispatcher) {
                this.dispatcher.Notify(Message.Completed, true);
            }
        }

        internal void _NotifyDestroy()
        {
            if (null != this.dispatcher) {
                this.dispatcher.Notify(Message.Destroy, true);
            }
        }

        private void _Clear()
        {
            if (null != this.objects) {
                for (var i = 0; i < this.objects.Count; ++i) {
                    this.objects[i].Dispose();
                }
                this.objects = null;
            }
        }

        private int index = 0;
        private SceneChunkRefData chunkRefData = null;
        private GameObject chunkRoot = null;
        private bool validateData = false;
        private float lifeTime = 0f;
        private List<SceneObject> objects = null;
        private Dispatcher dispatcher = null;
        private StateSet<States> stateSet = null;
        private StateMachine controller = null;

        private const float LIFE_TIME = 5f;

        // -------------
        internal float _DrawGraph(float yOffset)
        {
            GUI.DrawTexture(new Rect(0f, yOffset, GraphStyle.ServiceWidth, CHUNK_GRAPH_HEIGHT), GraphStyle.GreenTexture);
            GUI.Label(new Rect(0f, yOffset, GraphStyle.ServiceWidth, CHUNK_GRAPH_HEIGHT), this.chunkRefData.Path, GraphStyle.MiniLabel);
            yOffset += CHUNK_GRAPH_HEIGHT;
            if (null != this.objects) {
                for (var i = 0; i < this.objects.Count; ++i) {
                    yOffset = this.objects[i]._DrawGraph(yOffset);
                }
            }

            return yOffset;
        }

        internal float _GraphHeight
        {
            get {
                var height = CHUNK_GRAPH_HEIGHT;
                if (null != this.objects) {
                    for (var i = 0; i < this.objects.Count; ++i) {
                        height += this.objects[i]._GraphHeight;
                    }
                }

                return height;
            }
        }

        private const float CHUNK_GRAPH_HEIGHT = 20f;
    }
}