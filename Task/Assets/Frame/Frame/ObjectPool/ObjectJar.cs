// -----------------------------------------------------------------
// File:    ObjectJar.cs
// Author:  mouguangyi
// Date:    2016.12.08
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System.Collections.Generic;

namespace GameBox.Service.ObjectPool
{
    sealed class ObjectJar : C0
    {
        public ObjectJar()
        { }

        public override void Dispose()
        {
            this.jar = null;

            base.Dispose();
        }

        public object Pick()
        {
            return (this.jar.Count > 0 ? this.jar.Pop() : null);
        }

        public void Drop(object recycleObject)
        {
            this.jar.Push(recycleObject);

            if (this.jar.Count > this.maxCount) { this.maxCount = this.jar.Count; }
        }

        public int Count
        {
            get {
                return this.jar.Count;
            }
        }

        public int MaxCount
        {
            get {
                return this.maxCount;
            }
        }

        private Stack<object> jar = new Stack<object>();
        private int maxCount = 0;
    }
}