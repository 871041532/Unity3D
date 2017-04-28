// -----------------------------------------------------------------
// File:    IAssetDownloader.cs
// Author:  mouguangyi
// Date:    2016.05.26
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details �ʲ��ȸ�����������
    /// </summary>
    public interface IAssetDownloader
    {
        /// <summary>
        /// �ʲ��汾��
        /// </summary>
        string AssetVersion { get; }

        /// <summary>
        /// �ܹ���Ҫ���ش�С��
        /// </summary>
        long TotalSize { get; }

        /// <summary>
        /// �Ѿ����صĴ�С��
        /// </summary>
        long DownloadedSize { get; }

        /// <summary>
        /// �������ء�
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="parallelCount">ͬʱ�����ļ��ĸ�����Ĭ��ֵΪ-1����ʾ������Դͬʱ���ء�</param>
        void Start(Action callback, int parallelCount = -1);
    }
}