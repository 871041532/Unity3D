// -----------------------------------------------------------------
// File:    IElement.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// 
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// 
        /// </summary>
        object UserData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int Index { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IElement Find(string path, int type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IElement Clone(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void OnClick(Action handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void OnDragging(Action<float, float> handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        void MoveTo(int index);

        /// <summary>
        /// 
        /// </summary>
        void MoveToFirst();

        /// <summary>
        /// 
        /// </summary>
        void MoveToLast();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        void SetPosition(Vector3 pos);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Vector3 GetPosition();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        void SetScale(Vector3 scale);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Vector3 GetScale();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eulerAngles"></param>
        void SetEulerAngles(Vector3 eulerAngles);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Vector3 GetEulerAngles();
        /// <summary>
        /// 
        /// </summary>
        void Animate(string json, float duration = 1.0f, bool forever = false, bool relative = false, Action callBack = null);
    }
}
