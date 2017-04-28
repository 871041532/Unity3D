// -----------------------------------------------------------------
// File:    PrefabObject.cs
// Author:  mouguangyi
// Date:    2016.11.30
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
    public sealed class PrefabObject : EditorObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string PrefabPath;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override SceneObjectData ToData()
        {
            return new ScenePrefabData() {
                Type = SceneObjectType.PREFAB,
                Name = this.gameObject.name,
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
                Path = this.PrefabPath,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            Clear();

            var prefab = new ScenePrefab(new ScenePrefabData {
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
                Path = this.PrefabPath,
            }, this.gameObject);
            prefab.Load();
        }
    }
}