// -----------------------------------------------------------------
// File:    IControlFactory.cs
// Author:  mouguangyi
// Date:    2017.02.10
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IControlFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        IElement Create(int type, string path, RectTransform transform);
    }
}