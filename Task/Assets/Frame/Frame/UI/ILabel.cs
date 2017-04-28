// -----------------------------------------------------------------
// File:    ILabel.cs
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
    public interface ILabel : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int FontSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        HrefClickEvent OnHrefClick { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        void OnClick(Action<string> handler);
    }
}
