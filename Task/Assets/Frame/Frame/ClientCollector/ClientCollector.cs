// -----------------------------------------------------------------
// File:    ClientCollector.cs
// Author:  mouguangyi
// Date:    2016.04.21
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.GeoQuery;

namespace GameBox.Service.ClientCollector
{
    class ClientCollector : IClientCollector
    {
        public void Run(IServiceRunner runner)
        {
            this.infoPack = new ClientInfoPack();
            new ServiceTask("com.giant.service.geoquery")
            .Start()
            .Continue(task =>
            {
                var geoService = task.Result as IGeoQuery;
                return geoService.SimpleQuery();
            })
            .Continue(task =>
            {
                var result = task.Result as IGeoResult;
                if (0 == result.Status) {
                    this.infoPack.Nation = result.CountryCode;
                }
                return null;
            });

            runner.Ready(_Terminate);
        }

        public void Pulse(float delta)
        { }

        public string Id
        {
            get {
                return "com.giant.service.clientcollector";
            }
        }

        public IClientInfoPack Collect()
        {
            return this.infoPack;
        }

        private void _Terminate()
        { }

        private ClientInfoPack infoPack = null;
    }
}
