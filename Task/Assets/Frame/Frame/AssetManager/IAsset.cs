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
        /// ��Assetת�ɾ������Դ���͡�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Cast<T>();
    }
}