// -----------------------------------------------------------------
// File:    IAssetConfigure.cs
// Author:  mouguangyi
// Date:    2017.01.03
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details �ʲ������ļ��ӿڣ��������Ҫʵ�֡�
    /// </summary>
    public interface IAssetConfigure
    {
        /// <summary>
        /// �ʲ��汾�ţ���ʽ�����GameBox.Framework.Version���塣
        /// 
        /// @see GameBox.Framework.Version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// �ʲ������б�
        /// </summary>
        IAssetDescription[] Assets { get; }
    }
}