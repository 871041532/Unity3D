// -----------------------------------------------------------------
// File:    ScenePrefabData.cs
// Author:  mouguangyi
// Date:    2016.11.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// Format:
    ///     Type:
    ///     X, Y, Z:
    ///     Path: PrefabÎÄ¼þÂ·¾¶
    /// </summary>
    public sealed class ScenePrefabData : SceneObjectData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Write(TableStreamWriter writer)
        {
            base.Write(writer);

            writer.WriteKeyString("Path", this.Path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected override void Read(TableStreamReader reader)
        {
            base.Read(reader);

            this.Path = reader.ReadKeyString("Path");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected override void ReadAsync(TableStreamAsyncReader reader)
        {
            base.ReadAsync(reader);

            reader.ReadKeyString("Path", path => { this.Path = path; });
        }
    }
}