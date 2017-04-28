// -----------------------------------------------------------------
// File:    IButton.cs
// Author:  fuzhun
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
    public interface IButton : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        bool Interactable { get; set; }
    }
}
