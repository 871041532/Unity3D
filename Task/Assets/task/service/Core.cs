using System;

namespace GameFramework
{
    /// <summary>
    /// @details 基础类。
    /// </summary>
    public class C0 : IDisposable
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public C0()
        {
            this.runtimeId = (++C0.incrementId);
        }

        /// <summary>
        /// 析构函数。
        /// </summary>
        public virtual void Dispose()
        { }

        /// <summary>
        /// 运行时Id。
        /// </summary>
        protected int RuntimeId
        {
            get
            {
                return this.runtimeId;
            }
        }

        private int runtimeId = 0;

        private static int incrementId = 0;
    }
}
