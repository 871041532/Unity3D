using System;

namespace KeraLuaLite
{
    struct LuaTag
    {
        public LuaTag(IntPtr tag) : this()
        {
            this.Tag = tag;
        }

        static public implicit operator LuaTag(IntPtr ptr)
        {
            return new LuaTag(ptr);
        }

        public IntPtr Tag { get; set; }
    }
}
