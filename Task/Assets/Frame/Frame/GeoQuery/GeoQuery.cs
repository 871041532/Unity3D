// ---------------------------------------------------------------
// File: GeoQuery.cs
// Author: mouguangyi
// Date: 2016.03.25
// Description:
// 
// ---------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.GeoQuery
{
    /// <summary>
    /// 查询引擎类型。
    /// </summary>
    public enum GeoEngineType
    {
        /// <summary>
        /// 基于geoip.nekudo.com服务查询。
        /// </summary>
        SHINY,
    }

    sealed class GeoQuery : IGeoQuery
    {
        public GeoEngineType QueryEngineType
        {
            set { this.queryEngineType = value; }
        }

        public string Id
        {
            get { return "com.giant.service.geoquery"; }
        }

        public void Run(IServiceRunner runner)
        {
            _Initialize(this.queryEngineType);
            runner.Ready(_Terminate);
        }

        public void Pulse(float delta)
        { }

        public AsyncTask SimpleQuery(string ip = null)
        {
            return this.queryEngine.SimpleQuery(ip);
        }

        public AsyncTask Query(string ip = null)
        {
            return this.queryEngine.Query(ip);
        }

        private void _Terminate()
        { }

        private void _Initialize(GeoEngineType engineType)
        {
            if (null == this.queryEngine) {
                switch (engineType) {
                case GeoEngineType.SHINY:
                default:
                    this.queryEngine = new ShinyGeoQueryEngine();
                    break;
                }
            }
        }

        private GeoEngineType queryEngineType = GeoEngineType.SHINY;
        private IGeoQueryEngine queryEngine = null;
    }
}
