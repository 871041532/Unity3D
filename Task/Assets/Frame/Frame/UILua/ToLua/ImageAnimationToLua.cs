// -----------------------------------------------------------------
// File:    ImageAnimationToLua.cs
// Author:  liuwei
// Date:    2017.02.06
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Service.LuaRuntime;
using GameBox.Service.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameBox.Service.UIToLua
{
    class ImageAnimationToLua
    {
        public static void RegLuaBridgeFunction(ILuaRuntime luaRuntime)
        {
            luaRuntime.RegLuaBridgeFunction(ImageAnimationToLua.LuaPlaySpriteAnimation);
            luaRuntime.RegLuaBridgeFunction(ImageAnimationToLua.LuaPauseSpriteAnimation);
            luaRuntime.RegLuaBridgeFunction(ImageAnimationToLua.LuaSetSpriteAnimation);
        }

        [LuaBridge("image_playSpriteAnimation", 1, 0)]
        public static void LuaPlaySpriteAnimation(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var img = parameters[0] as IImageAnimation;
            if (null != img)
            {
                img.Play();
            }
        }

        [LuaBridge("image_pauseSpriteAnimation", 1, 0)]
        public static void LuaPauseSpriteAnimation(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var img = parameters[0] as IImageAnimation;
            if (null != img)
            {
                img.Pause();
            }
        }

        [LuaBridge("image_setSpriteAnimation", 2, 0)]
        public static void LuaSetSpriteAnimation(ILuaExecuter executer)
        {
            var parameters = executer.PopParameters();
            var img = parameters[0] as IImageAnimation;
            if (null != img && null != parameters[1])
            {
                ILuaTable spriteTb = parameters[1] as ILuaTable;
                string[] spritesTb = spriteTb.ToArray() as string[];
                List<SpriteLoadingInformation> spriteLoadingInformatonList = new List<SpriteLoadingInformation>();
                for (int i = 0; i < spritesTb.Length; i++)
                {
                    var spriteLoadingInformation = new SpriteLoadingInformation();
                    var spriteInfo = spritesTb[i];
                    var spriteInfoArray = spriteInfo.Split('|');
                    if (spriteInfoArray.Length < 2)
                        continue;
                    spriteLoadingInformation.path = spriteInfoArray[0];
                    spriteLoadingInformation.name = spriteInfoArray[1];
                    spriteLoadingInformatonList.Add(spriteLoadingInformation);
                }
                var framesPerSecond = parameters.Length > 2 && null != parameters[2] ? Convert.ToInt32(parameters[2]) : 10;
                img.SetSpriteAnimation(spriteLoadingInformatonList, framesPerSecond);
            }
        }
    }
}