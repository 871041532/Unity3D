// -----------------------------------------------------------------
// File:    IAsset.cs
// Author:  mouguangyi
// Date:    2017.03.07
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetManager
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAsset : IDisposable
    {
        /// <summary>
        /// 将Asset转成具体的资源类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Cast<T>();
    }
}