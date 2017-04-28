// -----------------------------------------------------------------
// File:    IAssetListUpdater.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System;

namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details �ʲ��ļ��б���·���ӿڡ�
    /// </summary>
    public interface IAssetListUpdater : IService
    {
        /// <summary>
        /// ���������ָ���ļ������������������ȸ���״̬��
        /// </summary>
        /// <param name="updateServerPath">���·����ַ</param>
        /// <param name="configureFile">�����ļ���</param>
        /// <param name="parser">�������Ҫʵ�ֵ��ʲ������ļ�������</param>
        /// <param name="callback"></param>
        void Check(string updateServerPath, string configureFile, IAssetConfigureParser parser, Action<AssetUpdateType, IAssetDownloader> callback);
    }
}


