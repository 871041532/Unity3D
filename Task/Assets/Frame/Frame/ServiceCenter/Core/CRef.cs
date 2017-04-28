// -----------------------------------------------------------------
// File:    CRef.cs
// Author:  mouguangyi
// Date:    2016.06.03
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// @details 基础引用计数类。
    /// </summary>
    public abstract class CRef<T>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CRef()
        { }

        /// <summary>
        /// 当前的引用计数。
        /// </summary>
        public int RefCount
        {
            get {
                return this.refCount;
            }
        }

        /// <summary>
        /// 加计数。
        /// </summary>
        public virtual void Retain()
        {
            ++this.refCount;
        }

        /// <summary>
        /// 减计数。
        /// </summary>
        public virtual void Release()
        {
            if (0 == (--this.refCount)) {
                Dispose();
            }
        }

        /// <summary>
        /// 计数为0时调用的析构函数。可重写。
        /// </summary>
        protected virtual void Dispose()
        { }

        private int refCount = 0;
    }
}
