// -----------------------------------------------------------------
// File:    ChunkDataLoadingState.cs
// Author:  mouguangyi
// Date:    2016.12.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    sealed class ChunkDataLoadingState : ChunkState
    {
        public ChunkDataLoadingState(SceneChunk.States stateId) : base(stateId)
        { }

        public override void Enter(StateMachine stateMachine)
        {
            this.completed = false;

            var chunk = stateMachine.Model as SceneChunk;
            chunk._Objects = new List<SceneObject>();
            ServiceCenter.GetService<IAssetManager>().LoadAsync(chunk._ChunkRefData.Path + ".bytes", AssetType.BYTES, asset =>
            {
                if (null != asset) {
                    var stream = new MemoryStream(asset.Cast<byte[]>());
                    SceneChunkData.DeserializeAsync(stream, chunk._ValidateData, chunkData =>
                    {
                        stream.Close();
                        stream.Dispose();
                        for (var i = 0; i < chunkData.Objects.Count; ++i) {
                            var go = new GameObject();
                            go.transform.SetParent(chunk._ChunkRoot.transform);

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
                            chunk._Objects.Add(obj);
                        }

                        this.completed = true;
                    });
                }
                asset.Dispose();
            });
        }

        public override void Execute(StateMachine stateMachine, float delta)
        {
            if (this.completed) {
                var chunk = stateMachine.Model as SceneChunk;
                chunk._ChangeState(SceneChunk.States.OBJECTLOADING);
            }
        }

        private bool completed = false;
    }
}