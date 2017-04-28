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
    /// @details �������ü����ࡣ
    /// </summary>
    public abstract class CRef<T>
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        public CRef()
        { }

        /// <summary>
        /// ��ǰ�����ü�����
        /// </summary>
        public int RefCount
        {
            get {
                return this.refCount;
            }
        }

        /// <summary>
        /// �Ӽ�����
        /// </summary>
        public virtual void Retain()
        {
            ++this.refCount;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public virtual void Release()
        {
            if (0 == (--this.refCount)) {
                Dispose();
            }
        }

        /// <summary>
        /// ����Ϊ0ʱ���õ���������������д��
        /// </summary>
        protected virtual void Dispose()
        { }

        private int refCount = 0;
    }
}
