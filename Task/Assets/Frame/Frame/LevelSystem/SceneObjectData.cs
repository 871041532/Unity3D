// -----------------------------------------------------------------
// File:    SceneObjectData.cs
// Author:  mouguangyi
// Date:    2016.11.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// 
    /// </summary>
    public enum SceneObjectType
    {
        /// <summary>
        /// 
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        /// 
        /// </summary>
        PREFAB = 0,

        /// <summary>
        /// 
        /// </summary>
        LIGHTMODIFIER = 1,
    }

    /// <summary>
    /// Format:
    ///     Type:
    ///     X, Y, Z:
    /// </summary>
    public class SceneObjectData
    {
        /// <summary>
        /// 
        /// </summary>
        public SceneObjectType Type { get; set; }

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
        public float ScaleX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float ScaleY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float ScaleZ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void Write(TableStreamWriter writer)
        {
            writer.WriteKeyByte("Type", Convert.ToByte(this.Type));

            writer.WriteKeyString("Name", this.Name);

            writer.WriteKeyFloat("PositionX", this.PositionX);
            writer.WriteKeyFloat("PositionY", this.PositionY);
            writer.WriteKeyFloat("PositionZ", this.PositionZ);

            writer.WriteKeyFloat("OrientationX", this.OrientationX);
            writer.WriteKeyFloat("OrientationY", this.OrientationY);
            writer.WriteKeyFloat("OrientationZ", this.OrientationZ);
            writer.WriteKeyFloat("OrientationW", this.OrientationW);

            writer.WriteKeyFloat("ScaleX", this.ScaleX);
            writer.WriteKeyFloat("ScaleY", this.ScaleY);
            writer.WriteKeyFloat("ScaleZ", this.ScaleZ);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected virtual void Read(TableStreamReader reader)
        {
            this.Type = (SceneObjectType)reader.ReadKeyByte("Type");

            this.Name = reader.ReadKeyString("Name");

            this.PositionX = (float)reader.ReadKeyFloat("PositionX");
            this.PositionY = (float)reader.ReadKeyFloat("PositionY");
            this.PositionZ = (float)reader.ReadKeyFloat("PositionZ");

            this.OrientationX = (float)reader.ReadKeyFloat("OrientationX");
            this.OrientationY = (float)reader.ReadKeyFloat("OrientationY");
            this.OrientationZ = (float)reader.ReadKeyFloat("OrientationZ");
            this.OrientationW = (float)reader.ReadKeyFloat("OrientationW");

            this.ScaleX = (float)reader.ReadKeyFloat("ScaleX");
            this.ScaleY = (float)reader.ReadKeyFloat("ScaleY");
            this.ScaleZ = (float)reader.ReadKeyFloat("ScaleZ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected virtual void ReadAsync(TableStreamAsyncReader reader)
        {
            reader.ReadKeyByte("Type", type => { this.Type = (SceneObjectType)type; });

            reader.ReadKeyString("Name", name => { this.Name = name; });

            reader.ReadKeyFloat("PositionX", value => { this.PositionX = (float)value; });
            reader.ReadKeyFloat("PositionY", value => { this.PositionY = (float)value; });
            reader.ReadKeyFloat("PositionZ", value => { this.PositionZ = (float)value; });

            reader.ReadKeyFloat("OrientationX", value => { this.OrientationX = (float)value; });
            reader.ReadKeyFloat("OrientationY", value => { this.OrientationY = (float)value; });
            reader.ReadKeyFloat("OrientationZ", value => { this.OrientationZ = (float)value; });
            reader.ReadKeyFloat("OrientationW", value => { this.OrientationW = (float)value; });

            reader.ReadKeyFloat("ScaleX", value => { this.ScaleX = (float)value; });
            reader.ReadKeyFloat("ScaleY", value => { this.ScaleY = (float)value; });
            reader.ReadKeyFloat("ScaleZ", value => { this.ScaleZ = (float)value; });
        }

        internal void _Write(TableStreamWriter writer)
        {
            writer.Blob("SceneObjectData", () =>
            {
                Write(writer);
            });
        }

        internal void _Read(TableStreamReader reader)
        {
            reader.Blob("SceneObjectData", () =>
            {
                Read(reader);
            });
        }

        internal void _ReadAsync(TableStreamAsyncReader reader)
        {
            reader.Blob("SceneObjectData", () =>
            {
                ReadAsync(reader);
            });
        }
    }
}