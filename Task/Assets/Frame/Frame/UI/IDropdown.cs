// -----------------------------------------------------------------
// File:    IDropdown.cs
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
    public interface IDropDown : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        int Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void OnClick(Action<int> handler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        void AddOption(string option);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string GetOption(int index);

        /// <summary>
        /// 
        /// </summary>
        void ClearOptions();
    }
}
