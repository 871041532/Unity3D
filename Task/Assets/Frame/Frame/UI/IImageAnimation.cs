// -----------------------------------------------------------------
// File:    IImageAnimation.cs
// Author:  liuwei
// Date:    2017.02.06
// Description:
//      
// -----------------------------------------------------------------
using System.Collections.Generic;

namespace GameBox.Service.UI
{
    public interface IImageAnimation : IElement
    {
        void Play();

        void Pause();

        /// <param name="spriteLoadingInformatonList"></param>
        /// <param name="framesPerSecond"></param>
        void SetSpriteAnimation(List<SpriteLoadingInformation> spriteLoadingInformatonList, int framesPerSecond);
    }
}
