// -----------------------------------------------------------------
// File:    LuaBase.cs
// Author:  mouguangyi
// Date:    2016.08.11
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.LuaRuntime
{
    abstract class LuaBase
    {
        public LuaBase(int reference)
        {
            this.reference = reference;
        }

        public int Reference
        {
            get {
                return this.reference;
            }
        }

        private int reference = -1;
    }
}