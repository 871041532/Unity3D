// -----------------------------------------------------------------
// File:    ImageToLua.cs
// Author:  fuzhun
// Date:    2016.09.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.LuaRuntime;
using GameBox.Service.UI;

namespace GameBox.Service.UIToLua
{
    class ImageToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(ImageToLua.LuaSetSprite);
            luaRuntime.RegLuaBridgeFunction(ImageToLua.LuaSetSpriteAsync);
            luaRuntime.RegLuaBridgeFunction(ImageToLua.LuaSetSpriteAtlas);
            luaRuntime.RegLuaBridgeFunction(ImageToLua.LuaSetSpriteAtlasAsync);
        }

        [LuaBridge("image_setSprite", 2, 0)]
        public static void LuaSetSprite(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var img = parameters[0] as IImage;
            if (null != img && null != parameters[1]) {
                img.SetSprite(parameters[1].ToString(), (parameters.Length > 2 && null != parameters[2] ? parameters[2].ToString() : ""));
            }
        }

        [LuaBridge("image_setSpriteAsync", 2, 0)]
        public static void LuaSetSpriteAsync(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var img = parameters[0] as IImage;
            if (null != img && null != parameters[1]) {
                var defaultPath = parameters.Length > 2 && null != parameters[2] ? parameters[2].ToString() : "";
                img.SetSpriteAsync(parameters[1].ToString(), defaultPath, () =>
                {
                    var callback = parameters.Length > 3 ? parameters[3] : null;
                    if (null != callback) {
                        executer.Call(callback);
                    }
                });
            }
        }

        [LuaBridge("image_setSpriteAtlas", 3, 0)]
        public static void LuaSetSpriteAtlas(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var img = parameters[0] as IImage;
            if (null != img && null != parameters[1] && null != parameters[2]) {
                var defaultPath = parameters.Length > 3 && null != parameters[3] ? parameters[3].ToString() : "";
                img.SetSpriteAtlas(parameters[1].ToString(), parameters[2].ToString(), defaultPath);
            }
        }

        [LuaBridge("image_setSpriteAtlasAsync", 3, 0)]
        public static void LuaSetSpriteAtlasAsync(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var img = parameters[0] as IImage;
            if (null != img && null != parameters[1] && null != parameters[2]) {
                var defaultPath = parameters.Length > 3 && null != parameters[3] ? parameters[3].ToString() : "";
                img.SetSpriteAtlasAsync(parameters[1].ToString(), parameters[2].ToString(), defaultPath, () =>
                {
                    var callback = parameters.Length > 4 ? parameters[4] : null;
                    if (null != callback) {
                        executer.Call(callback);
                    }
                });
            }
        }
    }
}