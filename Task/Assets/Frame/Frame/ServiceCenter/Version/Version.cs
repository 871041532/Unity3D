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
    /// @details ��Ϸ�汾����
    /// </summary>
    public sealed class Version
    {
        /// <summary>
        /// �������汾��
        /// </summary>
        public const int IGNORE_MAJOR = 0x1;

        /// <summary>
        /// ����С�汾��
        /// </summary>
        public const int IGNORE_MINOR = 0x2;

        /// <summary>
        /// Ĭ�Ϲ��캯����
        /// </summary>
        public Version()
        {
            this.major = 0;
            this.minor = 0;
            this.build = 0;
            this.revision = 0;
        }

        /// <summary>
        /// ���ݰ汾��Ϣ���캯����
        /// </summary>
        /// <param name="major">���汾��</param>
        /// <param name="minor">С�汾��</param>
        public Version(int major, int minor)
        {
            this.major = major;
            this.minor = minor;
            this.build = 0;
            this.revision = 0;
        }

        /// <summary>
        /// ���ݰ汾��Ϣ�͹����Ź��캯����
        /// </summary>
        /// <param name="major">���汾��</param>
        /// <param name="minor">С�汾��</param>
        /// <param name="build">�����汾��</param>
        public Version(int major, int minor, int build)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = 0;
        }

        /// <summary>
        /// ���ݰ汾��Ϣ,�����ź�Revision�Ź��캯����
        /// </summary>
        /// <param name="major">���汾��</param>
        /// <param name="minor">С�汾��</param>
        /// <param name="build">�����汾��</param>
        /// <param name="revision">Revision�汾��</param>
        public Version(int major, int minor, int build, int revision)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = revision;
        }

        /// <summary>
        /// ���ݰ汾�ַ������캯����
        /// </summary>
        /// <param name="version">�汾�ַ�����</param>
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
        /// �Ƚϰ汾��С��
        /// </summary>
        /// <param name="version">���Ƚϵİ汾��</param>
        /// <param name="options">�Ƚ�ѡ�IGNORE_MAJOR�� IGNORE_MINOR����ʹ��Ĭ��ֵ��</param>
        /// <returns>���رȽϽ����1��ʾ���ڣ�-1��ʾС�ڣ�0��ʾ��ȡ�</returns>
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
        /// ���汾�š�
        /// </summary>
        public int Major
        {
            get {
                return this.major;
            }
        }

        /// <summary>
        /// С�汾�š�
        /// </summary>
        public int Minor
        {
            get {
                return this.minor;
            }
        }

        /// <summary>
        /// �����汾�š�
        /// </summary>
        public int Build
        {
            get {
                return this.build;
            }
        }

        /// <summary>
        /// Revision�汾�š�
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