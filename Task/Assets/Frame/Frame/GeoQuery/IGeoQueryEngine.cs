// ---------------------------------------------------------------
// File: IGeoQueryEngine.cs
// Author: mouguangyi
// Date: 2016.04.21
// Description:
//   Interface for Geo query engine
// ---------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.GeoQuery
{
    interface IGeoQueryEngine
    {
        AsyncTask SimpleQuery(string ip);
        AsyncTask Query(string ip);
    }
}
