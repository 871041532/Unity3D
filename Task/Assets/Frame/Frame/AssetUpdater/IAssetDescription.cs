// -----------------------------------------------------------------
// File:    IAssetDescription.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details �����ʲ��ļ��������������Ҫʵ�֡�
    /// </summary>
    public interface IAssetDescription
    {
        /// <summary>
        /// �ļ�·����
        /// </summary>
        string Path { get; }

        /// <summary>
        /// �ļ���С��
        /// </summary>
        long Size { get; }

        /// <summary>
        /// У���ļ������Ƿ�һ�¡�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Equals(byte[] data);
    }
}