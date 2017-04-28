// -----------------------------------------------------------------
// File:    ChunkObject.cs
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
    public sealed class ChunkObject : EditorObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path = "";

        /// <summary>
        /// 
        /// </summary>
        public void Awake()
        {
            this.lockPosition = new Vector2();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            this.lockPosition.Set(this.transform.localPosition.x, this.transform.localPosition.z);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            this.transform.localPosition = new Vector3(this.lockPosition.x, this.transform.localPosition.y, this.lockPosition.y);
            this.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            Clear();

            var chunk = new SceneChunk(0, new SceneChunkRefData {
                PositionX = this.transform.localPosition.x,
                PositionY = this.transform.localPosition.y,
                PositionZ = this.transform.localPosition.z,
                OrientationX = this.transform.localRotation.x,
                OrientationY = this.transform.localRotation.y,
                OrientationZ = this.transform.localRotation.z,
                OrientationW = this.transform.localRotation.w,
                Path = this.Path,
            }, this.gameObject, true);
            chunk.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.7f);
            Gizmos.DrawCube(new Vector3(this.lockPosition.x, this.transform.localPosition.y, this.lockPosition.y), Vector3.one);
        }

        /// <summary>
        /// 
        /// </summary>
        internal Vector2 _LockPosition
        {
            set {
                this.lockPosition = value;
                this.transform.localPosition = new Vector3(this.lockPosition.x, this.transform.localPosition.y, this.lockPosition.y);
            }
        }

        private Vector2 lockPosition;
    }
}