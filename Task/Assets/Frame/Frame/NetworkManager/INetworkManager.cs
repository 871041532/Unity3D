// -----------------------------------------------------------------
// File:    INetworkManager.cs
// Author:  mouguangyi
// Date:    2016.05.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.NetworkManager
{
    /// <summary>
    /// ���������񡣷�װ����������ײ�ĸ���ͨ�ŷ���
    /// </summary>
    public interface INetworkManager : IService
    {
        /// <summary>
        /// ����ͨ�����ʹ���ͨ�ŵ�ͨ����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        INetworkChannel Create(string type);
    }
}