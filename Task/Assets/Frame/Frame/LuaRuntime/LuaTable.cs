// -----------------------------------------------------------------
// File:    LuaTable.cs
// Author:  mouguangyi
// Date:    2016.08.11
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;

namespace GameBox.Service.LuaRuntime
{
    using LuaState = KeraLuaLite.LuaState;
    using LuaLib = NLuaLite.LuaLib;

    class LuaTable : LuaBase, ILuaTable
    {
        public LuaTable(LuaState luaState, int reference) : base(reference)
        {
            this.luaState = luaState;
        }

        public object[] ToArray()
        {
            var list = new List<object>();
            int oldTop = LuaLib.LuaGetTop(luaState);
            try {
                LuaLib.LuaGetRef(this.luaState, this.Reference);
                LuaLib.LuaPushNil(this.luaState);

                while (0 != LuaLib.LuaNext(this.luaState, -2)) {
                    list.Add(LuaExecuter._ParseLuaValue(this.luaState, -1));
                    LuaLib.LuaSetTop(this.luaState, -2);
                }
            } finally {
                LuaLib.LuaSetTop(this.luaState, oldTop);
            }

            return list.ToArray();
        }

        public Dictionary<object, object> ToMap()
        {
            var map = new Dictionary<object, object>();
            int oldTop = LuaLib.LuaGetTop(luaState);
            try {
                LuaLib.LuaGetRef(this.luaState, this.Reference);
                LuaLib.LuaPushNil(this.luaState);

                while (0 != LuaLib.LuaNext(this.luaState, -2)) {
                    map[LuaExecuter._ParseLuaValue(this.luaState, -2)] = LuaExecuter._ParseLuaValue(this.luaState, -1);
                    LuaLib.LuaSetTop(this.luaState, -2);
                }
            } finally {
                LuaLib.LuaSetTop(this.luaState, oldTop);
            }

            return map;
        }

        private LuaState luaState;
    }
}