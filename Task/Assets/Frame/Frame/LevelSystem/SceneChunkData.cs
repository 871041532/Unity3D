// -----------------------------------------------------------------
// File:    SceneChunkData.cs
// Author:  mouguangyi
// Date:    2016.11.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// Chunk file format:
    ///     Name: 区块名字
    ///     Objects:
    ///         - Object
    ///             Type:
    ///             X, Y, Z:
    ///         - Object
    ///             Type:
    ///             X, Y, Z:
    ///         ...
    /// </summary>
    /// 
    public sealed class SceneChunkData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SceneObjectData> Objects { get; set; }

        private void _Write(TableStreamWriter writer)
        {
            writer.Blob("SceneChunkData", () =>
            {
                writer.WriteKeyString("Name", this.Name);

                writer.WriteKeyNumber("Objects", this.Objects.Count);
                for (var i = 0; i < this.Objects.Count; ++i) {
                    var objectData = this.Objects[i];
                    writer.WriteKeyByte("Type", Convert.ToByte(objectData.Type));
                    objectData._Write(writer);
                }
            });
        }

        private void _Read(TableStreamReader reader)
        {
            reader.Blob("SceneChunkData", () =>
            {
                this.Name = reader.ReadKeyString("Name");

                this.Objects = new List<SceneObjectData>();
                var count = reader.ReadKeyNumber("Objects");
                for (var i = 0; i < count; ++i) {
                    var type = (SceneObjectType)reader.ReadKeyByte("Type");
                    SceneObjectData objectData = null;
                    switch (type) {
                    case SceneObjectType.PREFAB:
                        objectData = new ScenePrefabData();
                        break;
                    case SceneObjectType.LIGHTMODIFIER:
                        objectData = new SceneLightModifierData();
                        break;
                    default:
                        objectData = new SceneObjectData();
                        break;
                    }
                    objectData._Read(reader);
                    this.Objects.Add(objectData);
                }
            });
        }

        private void _ReadAsync(TableStreamAsyncReader reader)
        {
            reader.Blob("SceneChunkData", () =>
            {
                reader.ReadKeyString("Name", name => { this.Name = name; });

                this.Objects = new List<SceneObjectData>();
                reader.ReadKeyNumber("Objects", count =>
                {
                    for (var i = 0; i < count; ++i) {
                        reader.ReadKeyByte("Type", type =>
                        {
                            SceneObjectData objectData = null;
                            switch ((SceneObjectType)type) {
                            case SceneObjectType.PREFAB:
                                objectData = new ScenePrefabData();
                                break;
                            case SceneObjectType.LIGHTMODIFIER:
                                objectData = new SceneLightModifierData();
                                break;
                            default:
                                objectData = new SceneObjectData();
                                break;
                            }
                            objectData._ReadAsync(reader);
                            this.Objects.Add(objectData);
                        });
                    }
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="chunkData"></param>
        public static void Serialize(Stream stream, SceneChunkData chunkData)
        {
            using (var writer = new TableStreamWriter(stream)) {
                chunkData._Write(writer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="validateData">是否校验数据</param>
        /// <returns></returns>
        public static SceneChunkData Deserialize(Stream stream, bool validateData)
        {
            using (var reader = new TableStreamReader(stream, validateData)) {
                var chunkData = new SceneChunkData();
                chunkData._Read(reader);
                return chunkData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="validateData"></param>
        /// <param name="callback"></param>
        public static void DeserializeAsync(Stream stream, bool validateData, Action<SceneChunkData> callback)
        {
            var reader = new TableStreamAsyncReader(stream, validateData);
            var chunkData = new SceneChunkData();
            chunkData._ReadAsync(reader);
            reader.StartAsync(READ_COUNT_PER_FRAME, () =>
            {
                if (null != callback) {
                    callback(chunkData);
                }
            });
        }

        private const int READ_COUNT_PER_FRAME = 100;   // Read action count per frame
    }
}

