// -----------------------------------------------------------------
// File:    LevelObject.cs
// Author:  mouguangyi
// Date:    2016.11.30
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    /// <summary>
    /// 
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public sealed class LevelObject : EditorObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path = "";

        /// <summary>
        /// 
        /// </summary>
        public float ChunkWidth = 1;

        /// <summary>
        /// 
        /// </summary>
        public float ChunkHeight = 1;

        /// <summary>
        /// 
        /// </summary>
        public int ChunkColumns = 1;

        /// <summary>
        /// 
        /// </summary>
        public int ChunkRows = 1;

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            Clear();

            var level = new SceneLevel(this.Path, this.gameObject, true);
            level.Load(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            var count = ChunkRows * ChunkColumns;

            for (var i = 0; i < count; ++i) {
                Transform trans = null;
                if (i >= this.transform.childCount) {
                    var go = new GameObject("_Chunk", new Type[] {
                        typeof(ChunkObject),
                    });
                    trans = go.transform;
                    trans.SetParent(this.transform);
                } else {
                    trans = this.transform.GetChild(i);
                }

                var chunComponent = trans.GetComponent<ChunkObject>();
                chunComponent._LockPosition = new Vector2(i / ChunkColumns * ChunkWidth, i % ChunkColumns * ChunkHeight);
            }

            if (count < this.transform.childCount) {
                for (var i = count; i < this.transform.childCount; ++i) {
                    GameObject.DestroyImmediate(this.transform.GetChild(i).gameObject);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            for (var i = 0; i < this.ChunkColumns + 1; ++i) {
                Gizmos.DrawLine(new Vector3((i - 0.5f) * this.ChunkWidth, 0, -0.5f * this.ChunkHeight), new Vector3((i - 0.5f) * this.ChunkWidth, 0, (this.ChunkRows - 0.5f) * this.ChunkHeight));
            }
            for (var i = 0; i < this.ChunkRows + 1; ++i) {
                Gizmos.DrawLine(new Vector3(-0.5f * this.ChunkWidth, 0, (i - 0.5f) * this.ChunkHeight), new Vector3((this.ChunkColumns - 0.5f) * this.ChunkWidth, 0, (i - 0.5f) * this.ChunkHeight));
            }
        }
    }
}