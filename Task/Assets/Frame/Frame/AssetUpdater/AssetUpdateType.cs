// -----------------------------------------------------------------
// File:    AssetUpdateType.cs
// Author:  mouguangyi
// Date:    2016.08.29
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Service.AssetUpdater
{
    /// <summary>
    /// @details �ȸ���״̬���͡�
    /// </summary>
    public enum AssetUpdateType
    {
        /// <summary>
        /// ��Ч��
        /// </summary>
        INVALID = -1,

        /// <summary>
        /// �Ѹ��¡�
        /// </summary>
        UPDATED = 0,

        /// <summary>
        /// ��Ҫ�ȸ��¡�
        /// </summary>
        HOTUPDATE = 1,

        /// <summary>
        /// ��Ҫȫ�汾ǿ���¡�
        /// </summary>
        FULLUPDATE = 2,
    }
}