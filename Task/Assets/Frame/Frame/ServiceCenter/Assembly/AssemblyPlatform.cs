// -----------------------------------------------------------------
// File:    AssemblyPlatform.cs
// Author:  mouguangyi
// Date:    2017.03.07
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SupportPlatform
    {
        /// <summary>
        /// 编辑器。
        /// </summary>
        public const int EDITOR = 0x1;

        /// <summary>
        /// PC。
        /// </summary>
        public const int STANDALONE = 0x2;

        /// <summary>
        /// iOS。
        /// </summary>
        public const int IOS = 0x4;

        /// <summary>
        /// Android。
        /// </summary>
        public const int ANDROID = 0x8;

        /// <summary>
        /// 任意平台。
        /// </summary>
        public const int ANY = 0xffff;
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AssemblyPlatformAttribute : Attribute
    {
        public AssemblyPlatformAttribute(int supportPlatforms)
        {
            this.supportPlatforms = supportPlatforms;
        }

        public int SupportPlatforms
        {
            get {
                return this.supportPlatforms;
            }
        }

        private int supportPlatforms = -1;
    }
}