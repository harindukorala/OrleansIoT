using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Shared.Monitoring;

namespace SiloCore.Monitoring
{
    class SiloMonitoringHolder : Grain, ISiloMonitoringHolder
    {
        public Task AddInformation(SiloMonitoringInformation information)
        {
            informations.Add(information);
            return Task.CompletedTask;
        }

        public Task<IList<SiloMonitoringInformation>> GetInformation()
        {
            return Task.FromResult<IList<SiloMonitoringInformation>>(informations);
        }

        private List<SiloMonitoringInformation> informations;
    }
}
