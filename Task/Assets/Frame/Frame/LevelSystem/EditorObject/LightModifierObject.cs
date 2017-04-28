// -----------------------------------------------------------------
// File:    LightModifierObject.cs
// Author:  mouguangyi
// Date:    2016.12.12
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// 
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public sealed class LightModifierObject : EditorObject
    {
        /// <summary>
        /// 
        /// </summary>
        public float Intensity = 1.0f;

        /// <summary>
        /// 
        /// </summary>
        public Color Color = Color.white;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SceneObjectData ToData()
        {
            return new SceneLightModifierData() {
                Type = SceneObjectType.LIGHTMODIFIER,
                Name = this.transform.gameObject.name,
                PositionX = this.transform.localPosition.x,
                PositionY = this.transform.localPosition.y,
                PositionZ = this.transform.localPosition.z,
                OrientationX = this.transform.localRotation.x,
                OrientationY = this.transform.localRotation.y,
                OrientationZ = this.transform.localRotation.z,
                OrientationW = this.transform.localRotation.w,
                ScaleX = this.transform.localScale.x,
                ScaleY = this.transform.localScale.y,
                ScaleZ = this.transform.localScale.z,
                Intensity = this.Intensity,
                Red = this.Color.r,
                Green = this.Color.g,
                Blue = this.Color.b,
            };
        }
    }
}