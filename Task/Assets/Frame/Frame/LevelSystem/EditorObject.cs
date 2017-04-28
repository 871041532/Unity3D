// -----------------------------------------------------------------
// File:    EditorObject.cs
// Author:  mouguangyi
// Date:    2016.12.02
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
    public abstract class EditorObject : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual SceneObjectData ToData()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Clear()
        {
            var childCount = this.transform.childCount;
            if (childCount > 0) {
                for (var i = childCount - 1; i >= 0; --i) {
                    GameObject.DestroyImmediate(this.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}