// -----------------------------------------------------------------
// File:    Version.cs
// Author:  mouguangyi
// Date:    2016.09.19
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details 游戏版本对象。
    /// </summary>
    public sealed class Version
    {
        /// <summary>
        /// 忽略主版本。
        /// </summary>
        public const int IGNORE_MAJOR = 0x1;

        /// <summary>
        /// 忽略小版本。
        /// </summary>
        public const int IGNORE_MINOR = 0x2;

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public Version()
        {
            this.major = 0;
            this.minor = 0;
            this.build = 0;
            this.revision = 0;
        }

        /// <summary>
        /// 根据版本信息构造函数。
        /// </summary>
        /// <param name="major">主版本。</param>
        /// <param name="minor">小版本。</param>
        public Version(int major, int minor)
        {
            this.major = major;
            this.minor = minor;
            this.build = 0;
            this.revision = 0;
        }

        /// <summary>
        /// 根据版本信息和构建号构造函数。
        /// </summary>
        /// <param name="major">主版本。</param>
        /// <param name="minor">小版本。</param>
        /// <param name="build">构建版本。</param>
        public Version(int major, int minor, int build)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = 0;
        }

        /// <summary>
        /// 根据版本信息,构建号和Revision号构造函数。
        /// </summary>
        /// <param name="major">主版本。</param>
        /// <param name="minor">小版本。</param>
        /// <param name="build">构建版本。</param>
        /// <param name="revision">Revision版本。</param>
        public Version(int major, int minor, int build, int revision)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = revision;
        }

        /// <summary>
        /// 根据版本字符串构造函数。
        /// </summary>
        /// <param name="version">版本字符串。</param>
        public Version(string version)
        {
            var segments = version.Split('.');
            if (segments.Length >= 2) {
                this.major = int.Parse(segments[0]);
                this.minor = int.Parse(segments[1]);
            }
            if (segments.Length >= 3) {
                this.build = int.Parse(segments[2]);
            }
            if (segments.Length >= 4) {
                this.revision = int.Parse(segments[3]);
            }
        }

        /// <summary>
        /// 比较版本大小。
        /// </summary>
        /// <param name="version">待比较的版本。</param>
        /// <param name="options">比较选项，IGNORE_MAJOR， IGNORE_MINOR或者使用默认值。</param>
        /// <returns>返回比较结果。1表示大于；-1表示小于；0表示相等。</returns>
        public int Compare(Version version, int options = 0x0)
        {
            if (0 == (options & IGNORE_MAJOR)) {
                if (this.major > version.major) {
                    return 1;
                } else if (this.major < version.major) {
                    return -1;
                }
            }

            if (0 == (options & IGNORE_MINOR)) {
                if (this.minor > version.minor) {
                    return 1;
                } else if (this.minor < version.minor) {
                    return -1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 主版本号。
        /// </summary>
        public int Major
        {
            get {
                return this.major;
            }
        }

        /// <summary>
        /// 小版本号。
        /// </summary>
        public int Minor
        {
            get {
                return this.minor;
            }
        }

        /// <summary>
        /// 构建版本号。
        /// </summary>
        public int Build
        {
            get {
                return this.build;
            }
        }

        /// <summary>
        /// Revision版本号。
        /// </summary>
        public int Revision
        {
            get {
                return this.revision;
            }
        }

        private int major = 0;
        private int minor = 0;
        private int build = 0;
        private int revision = 0;
    }
}