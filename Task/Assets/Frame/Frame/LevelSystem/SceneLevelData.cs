// -----------------------------------------------------------------
// File:    SceneLevelData.cs
// Author:  mouguangyi
// Date:    2016.11.15
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
    /// Level file format:
    ///     Name: 关卡名字
    ///     ChunkWidth: 区块宽度
    ///     ChunkHeight: 区块高度
    ///     MinX:
    ///     MaxX:
    ///     MinY:
    ///     MaxY:
    ///     Chunks:
    ///         - Chunk
    ///             X, Y, Z: 区块坐标
    ///             Name: 区块文件
    ///         - Chunk
    ///             X, Y, Z: 区块坐标
    ///             Name: 区块文件
    ///         ...
    /// </summary>
    public sealed class SceneLevelData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float ChunkWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float ChunkHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ChunkColumns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ChunkRows { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SceneChunkRefData> Chunks { get; set; }

        private void _Write(TableStreamWriter writer)
        {
            writer.Blob("SceneLevelData", () =>
            {
                writer.WriteKeyString("Name", this.Name);

                writer.WriteKeyFloat("ChunkWidth", this.ChunkWidth);
                writer.WriteKeyFloat("ChunkHeight", this.ChunkHeight);

                writer.WriteKeyNumber("ChunkColumns", this.ChunkColumns);
                writer.WriteKeyNumber("ChunkRows", this.ChunkRows);

                writer.WriteKeyNumber("Chunks", this.Chunks.Count);
                for (var i = 0; i < this.Chunks.Count; ++i) {
                    this.Chunks[i]._Write(writer);
                }
            });
        }

        private void _Read(TableStreamReader reader)
        {
            reader.Blob("SceneLevelData", () =>
            {
                this.Name = reader.ReadKeyString("Name");

                this.ChunkWidth = (float)reader.ReadKeyFloat("ChunkWidth");
                this.ChunkHeight = (float)reader.ReadKeyFloat("ChunkHeight");

                this.ChunkColumns = (int)reader.ReadKeyNumber("ChunkColumns");
                this.ChunkRows = (int)reader.ReadKeyNumber("ChunkRows");

                this.Chunks = new List<SceneChunkRefData>();
                var count = reader.ReadKeyNumber("Chunks");
                for (var i = 0; i < count; ++i) {
                    var chunkRefData = new SceneChunkRefData();
                    chunkRefData._Read(reader);
                    this.Chunks.Add(chunkRefData);
                }
            });
        }

        private void _ReadAsync(TableStreamAsyncReader reader)
        {
            reader.Blob("SceneLevelData", () =>
            {
                reader.ReadKeyString("Name", name => { this.Name = name; });

                reader.ReadKeyFloat("ChunkWidth", chunkWidth => { this.ChunkWidth = (float)chunkWidth; });
                reader.ReadKeyFloat("ChunkHeight", chunkHeight => { this.ChunkHeight = (float)chunkHeight; });

                reader.ReadKeyNumber("ChunkColumns", chunkColumns => { this.ChunkColumns = (int)chunkColumns; });
                reader.ReadKeyNumber("ChunkRows", chunkRows => { this.ChunkRows = (int)chunkRows; });

                this.Chunks = new List<SceneChunkRefData>();
                reader.ReadKeyNumber("Chunks", count =>
                {
                    for (var i = 0; i < count; ++i) {
                        var chunkRefData = new SceneChunkRefData();
                        chunkRefData._ReadAsync(reader);
                        this.Chunks.Add(chunkRefData);
                    }
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="levelData"></param>
        public static void Serialize(Stream stream, SceneLevelData levelData)
        {
            using (var writer = new TableStreamWriter(stream)) {
                levelData._Write(writer);    
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="validateData">是否校验数据</param>
        /// <returns></returns>
        public static SceneLevelData Deserialize(Stream stream, bool validateData)
        {
            using (var reader = new TableStreamReader(stream, validateData)) {
                var levelData = new SceneLevelData();
                levelData._Read(reader);
                return levelData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="validateData"></param>
        /// <param name="callback"></param>
        public static void DeserializeAsync(Stream stream, bool validateData, Action<SceneLevelData> callback)
        {
            var reader = new TableStreamAsyncReader(stream, validateData);
            var levelData = new SceneLevelData();
            levelData._ReadAsync(reader);
            reader.StartAsync(100, () =>
            {
                if (null != callback) {
                    callback(levelData);
                }
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneChunkRefData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float PositionX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float PositionY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float PositionZ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float OrientationX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float OrientationY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float OrientationZ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float OrientationW { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        internal void _Write(TableStreamWriter writer)
        {
            writer.Blob("SceneChunkRefData", () =>
            {
                writer.WriteKeyString("Name", this.Name);

                writer.WriteKeyFloat("PositionX", this.PositionX);
                writer.WriteKeyFloat("PositionY", this.PositionY);
                writer.WriteKeyFloat("PositionZ", this.PositionZ);

                writer.WriteKeyFloat("OrientationX", this.OrientationX);
                writer.WriteKeyFloat("OrientationY", this.OrientationY);
                writer.WriteKeyFloat("OrientationZ", this.OrientationZ);
                writer.WriteKeyFloat("OrientationW", this.OrientationW);

                writer.WriteKeyString("Path", this.Path);
            });
        }

        internal void _Read(TableStreamReader reader)
        {
            reader.Blob("SceneChunkRefData", () =>
            {
                this.Name = reader.ReadKeyString("Name");

                this.PositionX = (float)reader.ReadKeyFloat("PositionX");
                this.PositionY = (float)reader.ReadKeyFloat("PositionY");
                this.PositionZ = (float)reader.ReadKeyFloat("PositionZ");

                this.OrientationX = (float)reader.ReadKeyFloat("OrientationX");
                this.OrientationY = (float)reader.ReadKeyFloat("OrientationY");
                this.OrientationZ = (float)reader.ReadKeyFloat("OrientationZ");
                this.OrientationW = (float)reader.ReadKeyFloat("OrientationW");

                this.Path = reader.ReadKeyString("Path");
            });
        }

        internal void _ReadAsync(TableStreamAsyncReader reader)
        {
            reader.Blob("SceneChunkRefData", () =>
            {
                reader.ReadKeyString("Name", name => { this.Name = name; });

                reader.ReadKeyFloat("PositionX", positionX => { this.PositionX = (float)positionX; });
                reader.ReadKeyFloat("PositionY", positionY => { this.PositionY = (float)positionY; });
                reader.ReadKeyFloat("PositionZ", positionZ => { this.PositionZ = (float)positionZ; });

                reader.ReadKeyFloat("OrientationX", orientationX => { this.OrientationX = (float)orientationX; });
                reader.ReadKeyFloat("OrientationY", orientationY => { this.OrientationY = (float)orientationY; });
                reader.ReadKeyFloat("OrientationZ", orientationZ => { this.OrientationZ = (float)orientationZ; });
                reader.ReadKeyFloat("OrientationW", orientationW => { this.OrientationW = (float)orientationW; });

                reader.ReadKeyString("Path", path => { this.Path = path; });
            });
        }
    }
}