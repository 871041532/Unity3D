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
    /// 地理位置查询的结果。
    /// </summary>
    public interface IGeoResult
    {
        /// <summary>
        /// 查询结果状态。0为成功；-1为失败。
        /// </summary>
        int Status { get; }

        /// <summary>
        /// 错误信息。
        /// </summary>
        string Error { get; }

        /// <summary>
        /// 国家码。
        /// </summary>
        string CountryCode { get; }

        /// <summary>
        /// 国家。
        /// </summary>
        string Country { get; }

        /// <summary>
        /// 州/省。
        /// </summary>
        string Subdivision { get; }

        /// <summary>
        /// 城市。
        /// </summary>
        string City { get; }
    }
}