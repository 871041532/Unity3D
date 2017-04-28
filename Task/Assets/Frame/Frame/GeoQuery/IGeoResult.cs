// ---------------------------------------------------------------
// File: IGeoResult.cs
// Author: mouguangyi
// Date: 2016.05.16
// Description:
//   Interface for Geo query engine
// ---------------------------------------------------------------
namespace GameBox.Service.GeoQuery
{
    /// <summary>
    /// ����λ�ò�ѯ�Ľ����
    /// </summary>
    public interface IGeoResult
    {
        /// <summary>
        /// ��ѯ���״̬��0Ϊ�ɹ���-1Ϊʧ�ܡ�
        /// </summary>
        int Status { get; }

        /// <summary>
        /// ������Ϣ��
        /// </summary>
        string Error { get; }

        /// <summary>
        /// �����롣
        /// </summary>
        string CountryCode { get; }

        /// <summary>
        /// ���ҡ�
        /// </summary>
        string Country { get; }

        /// <summary>
        /// ��/ʡ��
        /// </summary>
        string Subdivision { get; }

        /// <summary>
        /// ���С�
        /// </summary>
        string City { get; }
    }
}