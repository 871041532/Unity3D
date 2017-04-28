// -----------------------------------------------------------------
// File:    IImage.cs
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
    public interface IImage : IElement
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="defaultPath"></param>
        void SetSprite(string path, string defaultPath = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="defaultPath"></param>
        /// <param name="callback"></param>
        void SetSpriteAsync(string path, string defaultPath = "", Action callback = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="defaultPath"></param>
        void SetSpriteAtlas(string path, string name, string defaultPath = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="defaultPath"></param>
        /// <param name="callback"></param>
        void SetSpriteAtlasAsync(string path, string name, string defaultPath = "", Action callback = null);
    }
}
