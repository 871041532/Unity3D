// -----------------------------------------------------------------
// File:    IToggles.cs
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
    public interface IToggles : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        int ToggleOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void AddToggleChange(Action handler);
    }
}
