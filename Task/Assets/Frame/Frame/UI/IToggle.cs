// -----------------------------------------------------------------
// File:    IToggle.cs
// Author:  mouguangyi
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface IToggle : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        bool On { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void OnChange(Action<bool> handler);
    }
}