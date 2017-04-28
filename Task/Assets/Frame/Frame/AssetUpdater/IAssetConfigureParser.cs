// -----------------------------------------------------------------
// File:    IAssetConfigureParser.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details �ʲ������ļ�����������Ҫ�����ʵ�֡�
    /// </summary>
    public interface IAssetConfigureParser
    {
        /// <summary>
        /// �첽������
        /// </summary>
        /// <param name="data">��������</param>
        /// <param name="handler">����������Ӧ�����</param>
        /// <returns></returns>
        void ParseAsync(byte[] data, Action<IAssetConfigure> handler);
    }
}