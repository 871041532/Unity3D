// ---------------------------------------------------------------
// File: GeoResult.cs
// Author: mouguangyi
// Date: 2016.03.24
// Description:
//   Return structure with full information of Geo
// ---------------------------------------------------------------
namespace GameBox.Service.GeoQuery
{
    sealed class GeoResult : IGeoResult
    {
        public int Status { get; set; }
        public string Error { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string Subdivision { get; set; }
        public string City { get; set; }
    }
}
