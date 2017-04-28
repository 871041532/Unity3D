// -----------------------------------------------------------------
// File:    IAssetManagerUpdater.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetUpdater;
using System;

namespace GameBox.Service.AssetManagerUpdater
{
    /// <summary>
    /// @details �ʲ������ȸ��·���ӿڡ�
    /// </summary>
    public interface IAssetManagerUpdater : IService
    {
        /// <summary>
        /// Ӧ��ǿ���µİ�װ�����ط�������ַ��
        /// </summary>
        string AppServer { get; }

        /// <summary>
        /// �����ȸ��·���������ȡ����״̬����������
        /// </summary>
        /// <param name="callback"></param>
        void Check(Action<AssetUpdateType, IAssetDownloader> callback);
    }
}