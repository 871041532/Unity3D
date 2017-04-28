// -----------------------------------------------------------------
// File:    SceneLevel.cs
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
    sealed class SceneLevel : C0
    {
        public SceneLevel(string levelPath, GameObject levelRoot, bool validateData)
        {
            this.levelPath = levelPath;
            this.levelRoot = levelRoot;
            this.validateData = validateData;
        }

        public void Update(Vector3 position, Quaternion orientation, float delta)
        {
            if (null != this.chunks) {
                var chunk = _FindChunk(position.x, position.z);
                if (chunk != this.currentChunk) {
                    if (null != this.currentChunk) {
                        this.currentChunk.OnExit();
                        if (LevelLoadPolicy.SURROUNDING == this.policy) {
                            _UnloadFarChunksAsync(this.currentChunk.Index);
                        }
                    }

                    this.currentChunk = chunk;

                    if (null != this.currentChunk) {
                        this.currentChunk.OnEnter();
                        if (LevelLoadPolicy.SURROUNDING == this.policy) {
                            _LoadNearChunksAsync(this.currentChunk.Index);
                        }
                    }
                }

                for (var i = 0; i < this.chunks.Length; ++i) {
                    if (null != this.chunks[i]) {
                        this.chunks[i].OnUpdate(delta);
                    }
                }
            }
        }

        public override void Dispose()
        {
            if (!SceneLevel.IsInEdit()) {
                for (var i = 0; i < this.chunks.Length; ++i) {
                    if (null != this.chunks[i]) {
                        this.chunks[i].Dispose();
                    }
                }
                this.chunks = null;
            }
        }

        public void Load(Vector3 position, Quaternion orientation)
        {
            using (var asset = ServiceCenter.GetService<IAssetManager>().Load(levelPath + ".bytes", AssetType.BYTES)) {
                var bytes = asset.Cast<byte[]>();
                using (var stream = new MemoryStream(bytes)) {
                    // Parse data
                    var levelData = SceneLevelData.Deserialize(stream, this.validateData);
                    _InitData(levelData);

                    _LoadAllChunks();
                    this.currentChunk = _FindChunk(position.x, position.z);
                    this.currentChunk.OnEnter();
                }
            }
        }

        public void LoadAsync(Vector3 position, Quaternion orientation, LevelLoadPolicy policy, Action handler)
        {
            this.policy = policy;

            ServiceCenter.GetService<IAssetManager>().LoadAsync(levelPath + ".bytes", AssetType.BYTES, asset =>
            {
                if (null != asset) {
                    var stream = new MemoryStream(asset.Cast<byte[]>());
                    SceneLevelData.DeserializeAsync(stream, this.validateData, levelData =>
                    {
                        stream.Close();
                        stream.Dispose();

                        _InitData(levelData);

                        switch (this.policy) {
                        case LevelLoadPolicy.ALL:
                            _LoadAllChunksAsync(() =>
                            {
                                this.currentChunk = _FindChunk(position.x, position.z);
                                this.currentChunk.OnEnter();
                                _NotifyCompleted(handler);
                            });
                            break;
                        case LevelLoadPolicy.SURROUNDING:
                            // Start to load related chunks
                            this.currentChunk = _FindChunk(position.x, position.z);
                            if (null != this.currentChunk) {
                                this.currentChunk.LoadAsync((target, message) =>
                                {
                                    this.currentChunk.OnEnter();
                                    _NotifyCompleted(handler);
                                });
                            }

                            _LoadNearChunksAsync(this.currentChunk.Index);
                            break;
                        }

                    });
                }
                asset.Dispose();
            });
        }

        private void _InitData(SceneLevelData levelData)
        {
            this.chunkWidth = levelData.ChunkWidth;
            this.chunkHeight = levelData.ChunkHeight;
            this.chunkColumns = levelData.ChunkColumns;
            this.chunkRows = levelData.ChunkRows;
            this.chunks = new SceneChunk[this.chunkColumns * this.chunkRows];

            for (var i = 0; i < levelData.Chunks.Count; ++i) {
                var chunkRefData = levelData.Chunks[i];
                var x = chunkRefData.PositionX / this.chunkWidth;
                var z = chunkRefData.PositionZ  / this.chunkHeight;
                var index = Mathf.FloorToInt(z * this.chunkColumns + x);
                var go = new GameObject("_Chunk");
                go.transform.SetParent(this.levelRoot.transform);

                var chunk = new SceneChunk(index, chunkRefData, go, this.validateData);
                this.chunks[index] = chunk;
            }

            if (SceneLevel.IsInEdit()) {
                var levelObject = SceneLevel.GetOrAddComponent<LevelObject>(this.levelRoot);
                levelObject.Path = this.levelPath;
                levelObject.ChunkWidth = this.chunkWidth;
                levelObject.ChunkHeight = this.chunkHeight;
                levelObject.ChunkColumns = this.chunkColumns;
                levelObject.ChunkRows = this.chunkRows;
            }
        }

        private void _NotifyCompleted(Action handler)
        {
            if (null != handler) {
                handler();
            }
        }

        private void _LoadAllChunks()
        {
            var count = this.chunks.Length;
            for (var i = 0; i < this.chunks.Length; ++i) {
                this.chunks[i].Load();
            }
        }

        private void _LoadAllChunksAsync(Action handler)
        {
            var count = this.chunks.Length;
            for (var i = 0; i < this.chunks.Length; ++i) {
                this.chunks[i].LoadAsync((target, message) =>
                {
                    --count;
                    if (0 == count && null != handler) {
                        handler();
                    }
                });
            }
        }

        private void _LoadNearChunksAsync(int index)
        {
            var chunkQueue = new Queue<int>();

            if (null != this.chunks[index]) {
                chunkQueue.Enqueue(index);
            }

            var leftIndex = index - 1;
            if (leftIndex >= 0 && (leftIndex / this.chunkColumns) == (index / this.chunkColumns)) {
                if (null != this.chunks[leftIndex]) {
                    chunkQueue.Enqueue(leftIndex);
                }
            }

            var rightIndex = index + 1;
            if (rightIndex < this.chunks.Length && (rightIndex / this.chunkColumns) == (index / this.chunkColumns)) {
                if (null != this.chunks[rightIndex]) {
                    chunkQueue.Enqueue(rightIndex);
                }
            }

            var topIndex = index + this.chunkColumns;
            if (topIndex < this.chunks.Length) {
                if (null != this.chunks[topIndex]) {
                    chunkQueue.Enqueue(topIndex);
                }
            }

            var bottomIndex = index - this.chunkColumns;
            if (bottomIndex >= 0) {
                if (null != this.chunks[bottomIndex]) {
                    chunkQueue.Enqueue(bottomIndex);
                }
            }

            _LoadChunkQueueAsync(chunkQueue);
        }

        private void _LoadChunkQueueAsync(Queue<int> chunkQueue)
        {
            while (chunkQueue.Count > 0) {
                this.chunks[chunkQueue.Dequeue()].LoadAsync(null);
            }
        }

        private void _UnloadFarChunksAsync(int index)
        {
            var leftIndex = index - 1;
            if (leftIndex >= 0 && (leftIndex / this.chunkColumns) == (index / this.chunkColumns)) {
                this.chunks[leftIndex].UnloadAsync(null);
            }

            var rightIndex = index + 1;
            if (rightIndex < this.chunks.Length && (rightIndex / this.chunkColumns) == (index / this.chunkColumns)) {
                this.chunks[rightIndex].UnloadAsync(null);
            }

            var topIndex = index + this.chunkColumns;
            if (topIndex < this.chunks.Length) {
                this.chunks[topIndex].UnloadAsync(null);
            }

            var bottomIndex = index - this.chunkColumns;
            if (bottomIndex >= 0) {
                this.chunks[bottomIndex].UnloadAsync(null);
            }
        }

        private SceneChunk _FindChunk(float x, float z)
        {
            var index = Mathf.FloorToInt((z + this.chunkHeight * 0.5f) / this.chunkHeight) * this.chunkColumns + Mathf.FloorToInt((x + this.chunkWidth * 0.5f) / this.chunkWidth);
            return (index < 0 || index >= this.chunks.Length ? null : this.chunks[index]);
        }

        private string levelPath = null;
        private GameObject levelRoot = null;
        private bool validateData = false;
        private LevelLoadPolicy policy = LevelLoadPolicy.SURROUNDING;
        private float chunkWidth = 0;
        private float chunkHeight = 0;
        private int chunkColumns = 0;
        private int chunkRows = 0;
        private SceneChunk[] chunks = null;
        private SceneChunk currentChunk = null;

        public static bool IsInEdit()
        {
            return (Application.isEditor && !Application.isPlaying);
        }

        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            if (null == go) {
                return default(T);
            }

            var com = go.GetComponent<T>();
            if (null == com) {
                com = go.AddComponent<T>();
            }

            return com;
        }

        // -- IServiceGraph
        public void DrawGraph()
        {
            var yOffset = 0f;
            for (var i = 0; i < this.chunks.Length; ++i) {
                yOffset = this.chunks[i]._DrawGraph(yOffset);
            }
        }

        public float GraphWidth
        {
            get {
                return GraphStyle.ServiceWidth;
            }
        }

        public float GraphHeight
        {
            get {
                var height = 0f;
                for (var i = 0; i < this.chunks.Length; ++i) {
                    if (null != this.chunks[i]) {
                        height += this.chunks[i]._GraphHeight;
                    }
                }

                return height;
            }
        }
    }
}