// -----------------------------------------------------------------
// File:    SceneGroup.cs
// Author:  mouguangyi
// Date:    2016.12.13
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    class SceneGroup : SceneObject
    {
        public SceneGroup(SceneObjectData objectData, GameObject objectRoot) : base(objectData, objectRoot)
        { }

        public override void Load()
        {
            var go = new GameObject("_Folder");
            _Init(go);
        }

        public override void LoadAsync(Action handler)
        {
            Load();
            NotifyLoaded(handler);
        }

        private void _Init(GameObject go)
        {
            go.transform.SetParent(this.ObjectRoot.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }
    }
}