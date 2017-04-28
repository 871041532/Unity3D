using System;
using System.Runtime.InteropServices;

namespace KeraLuaLite
{
    /// <summary>
    /// Switch KeraLua interface back to lua5.1 version to support luajit library.
    /// </summary>
    static class NativeMethods
    {

#if UNITY_IOS
		const string LUALIBNAME = "__Internal";
        const string LUANETLIBNAME = "__Internal";
#else
        const string LUALIBNAME = "lua51";
        const string LUANETLIBNAME = "luanet";
#endif

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gc")]
        internal static extern int LuaGC(IntPtr luaState, int what, int data);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_typename")]
        internal static extern IntPtr LuaTypeName(IntPtr luaState, int type);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_error")]
        internal static extern void LuaLError(IntPtr luaState, string message);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_where")]
        internal static extern void LuaLWhere(IntPtr luaState, int level);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_newstate")]
        internal static extern IntPtr LuaLNewState();

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_close")]
        internal static extern void LuaClose(IntPtr luaState);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_openlibs")]
        internal static extern void LuaLOpenLibs(IntPtr luaState);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_loadstring")]
        internal static extern int LuaLLoadString(IntPtr luaState, string chunk);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_createtable")]
        internal static extern void LuaCreateTable(IntPtr luaState, int narr, int nrec);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gettable")]
        internal static extern void LuaGetTable(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_settop")]
        internal static extern void LuaSetTop(IntPtr luaState, int newTop);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_insert")]
        internal static extern void LuaInsert(IntPtr luaState, int newTop);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_remove")]
        internal static extern void LuaRemove(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawget")]
        internal static extern void LuaRawGet(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_settable")]
        internal static extern void LuaSetTable(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawset")]
        internal static extern void LuaRawSet(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_setmetatable")]
        internal static extern void LuaSetMetatable(IntPtr luaState, int objIndex);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_getmetatable")]
        internal static extern int LuaGetMetatable(IntPtr luaState, int objIndex);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushvalue")]
        internal static extern void LuaPushValue(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_replace")]
        internal static extern void LuaReplace(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gettop")]
        internal static extern int LuaGetTop(IntPtr luaState);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_type")]
        internal static extern int LuaType(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_ref")]
        internal static extern int LuaLRef(IntPtr luaState, int registryIndex);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawgeti")]
        internal static extern void LuaRawGetI(IntPtr luaState, int tableIndex, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawseti")]
        internal static extern void LuaRawSetI(IntPtr luaState, int tableIndex, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_newuserdata")]
        internal static extern IntPtr LuaNewUserData(IntPtr luaState, uint size);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_touserdata")]
        internal static extern IntPtr LuaToUserData(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_unref")]
        internal static extern void LuaLUnref(IntPtr luaState, int registryIndex, int reference);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_isstring")]
        internal static extern int LuaIsString(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_iscfunction")]
        internal static extern int LuaIsCFunction(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushnil")]
        internal static extern void LuaPushNil(IntPtr luaState);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pcall")]
        internal static extern int LuaPCall(IntPtr luaState, int nArgs, int nResults, int errfunc);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tocfunction")]
        internal static extern IntPtr LuaToCFunction(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tonumber")]
        internal static extern double LuaToNumber(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_toboolean")]
        internal static extern int LuaToBoolean(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_atpanic")]
        internal static extern void LuaAtPanic(IntPtr luaState, IntPtr panicf);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushnumber")]
        internal static extern void LuaPushNumber(IntPtr luaState, double number);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushboolean")]
        internal static extern void LuaPushBoolean(IntPtr luaState, int value);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tolstring")]
        internal static extern IntPtr LuaToLString(IntPtr luaState, int index, out uint strLen);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "lua_pushstring")]
        internal static extern void LuaPushString(IntPtr luaState, string str);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_newmetatable")]
        internal static extern int LuaLNewMetatable(IntPtr luaState, string meta);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "lua_getfield")]
        internal static extern void LuaGetField(IntPtr luaState, int stackPos, string meta);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_checkudata")]
        internal static extern IntPtr LuaLCheckUData(IntPtr luaState, int stackPos, string meta);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_getmetafield")]
        internal static extern int LuaLGetMetafield(IntPtr luaState, int stackPos, string field);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_loadbuffer")]
        internal static extern int LuaLLoadBuffer(IntPtr luaState, byte[] buff, uint size, string name);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luaL_loadfile")]
        internal static extern int LuaLLoadFile(IntPtr luaState, string filename);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_error")]
        internal static extern void LuaError(IntPtr luaState);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_checkstack")]
        internal static extern int LuaCheckStack(IntPtr luaState, int extra);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_next")]
        internal static extern int LuaNext(IntPtr luaState, int index);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushlightuserdata")]
        internal static extern void LuaPushLightUserData(IntPtr luaState, IntPtr udata);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gethookmask")]
        internal static extern int LuaGetHookMask(IntPtr luaState);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gethookcount")]
        internal static extern int LuaGetHookCount(IntPtr luaState);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_getinfo")]
        internal static extern int LuaGetInfo(IntPtr luaState, string what, IntPtr ar);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_getstack")]
        internal static extern int LuaGetStack(IntPtr luaState, int level, IntPtr n);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_getlocal")]
        internal static extern IntPtr LuaGetLocal(IntPtr luaState, IntPtr ar, int n);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_setlocal")]
        internal static extern IntPtr LuaSetLocal(IntPtr luaState, IntPtr ar, int n);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_getupvalue")]
        internal static extern IntPtr LuaGetUpValue(IntPtr luaState, int funcindex, int n);

        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_setupvalue")]
        internal static extern IntPtr LuaSetUpValue(IntPtr luaState, int funcindex, int n);

        // Add more bridges from lua5.1.
        // - mouguangyi
        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_objlen")]
        internal static extern int LuaObjLen(IntPtr luaState, int index);

        // - mouguangyi
        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushlstring")]
        internal static extern void LuaPushLString(IntPtr luaState, byte[] str, int size);

        // - mouguangyi
        [DllImport(LUALIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushcclosure")]
        internal static extern void LuaPushCClosure(IntPtr luaState, IntPtr function, int n);

        // --- LuaNet ---

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_pushstdcallcfunction")]
        internal static extern void LuaNetPushStdCallCFunction(IntPtr luaState, IntPtr function);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_checkmetatable")]
        internal static extern int LuaNetCheckMetatable(IntPtr luaState, int obj);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_equal")]
        internal static extern int LuaNetEqual(IntPtr luaState, int index1, int index2);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_isstring_strict")]
        internal static extern int LuaNetIsStringStrict(IntPtr luaState, int index);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_sethook")]
        internal static extern int LuaSetHook(IntPtr luaState, IntPtr func, int mask, int count);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_tonetobject")]
        internal static extern int LuaNetToNetObject(IntPtr luaState, int index);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_newudata")]
        internal static extern void LuaNetNewUData(IntPtr luaState, int val);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_rawnetobj")]
        internal static extern int LuaNetRawNetObj(IntPtr luaState, int obj);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luanet_checkudata")]
        internal static extern int LuaNetCheckUData(IntPtr luaState, int ud, string tname);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_gettag")]
        internal static extern IntPtr LuaNetGetTag();

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_pushglobaltable")]
        internal static extern void LuaNetPushGlobalTable(IntPtr luaState);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_popglobaltable")]
        internal static extern void LuaNetPopGlobalTable(IntPtr luaState);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luanet_getglobal")]
        internal static extern void LuaNetGetGlobal(IntPtr luaState, string name);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "luanet_setglobal")]
        internal static extern void LuaNetSetGlobal(IntPtr luaState, string name);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_registryindex")]
        internal static extern int LuaNetRegistryIndex();

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_get_main_state")]
        internal static extern IntPtr LuaNetGetMainState(IntPtr luaState);

        [DllImport(LUANETLIBNAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luanet_upvalueindex")]
        internal static extern int LuaNetUpValueIndex(int index);
    }
}
