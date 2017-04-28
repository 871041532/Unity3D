using System;

namespace KeraLuaLite
{
    struct LuaState
    {
        public LuaState(IntPtr ptrState) : this()
        {
            state = ptrState;
        }

        static public implicit operator LuaState(IntPtr ptr)
        {
            return new LuaState(ptr);
        }

        static public implicit operator IntPtr(LuaState luastate)
        {
            return luastate.state;
        }

        private IntPtr state;
    }
}
