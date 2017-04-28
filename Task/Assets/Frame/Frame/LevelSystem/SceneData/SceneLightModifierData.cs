// -----------------------------------------------------------------
// File:    SceneLightModifierData.cs
// Author:  mouguangyi
// Date:    2016.12.12
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneLightModifierData : SceneObjectData
    {
        /// <summary>
        /// 
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Red { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Green { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Blue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Write(TableStreamWriter writer)
        {
            base.Write(writer);

            writer.WriteKeyFloat("Intensity", this.Intensity);

            writer.WriteKeyFloat("Red", this.Red);
            writer.WriteKeyFloat("Green", this.Green);
            writer.WriteKeyFloat("Blue", this.Blue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected override void Read(TableStreamReader reader)
        {
            base.Read(reader);

            this.Intensity = (float)reader.ReadKeyFloat("Intensity");

            this.Red = (float)reader.ReadKeyFloat("Red");
            this.Green = (float)reader.ReadKeyFloat("Green");
            this.Blue = (float)reader.ReadKeyFloat("Blue");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected override void ReadAsync(TableStreamAsyncReader reader)
        {
            base.ReadAsync(reader);

            reader.ReadKeyFloat("Intensity", intensity => { this.Intensity = (float)intensity; });

            reader.ReadKeyFloat("Red", red => { this.Red = (float)red; });
            reader.ReadKeyFloat("Green", green => { this.Green = (float)green; });
            reader.ReadKeyFloat("Blue", blue => { this.Blue = (float)blue; });
        }
    }
}