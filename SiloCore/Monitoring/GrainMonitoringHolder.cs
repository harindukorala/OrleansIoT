using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;

using Shared.Monitoring;

namespace GrainCore.Monitoring
{
    class GrainMonitoringHolder : Grain, IGrainMonitoringHolder
    {
        public Task AddInformation(int interfaceId, GrainMonitoringInformation information)
        {
            List<GrainMonitoringInformation> interfaceInformations;
            if(!informations.TryGetValue(interfaceId, out interfaceInformations))
            {
                interfaceInformations = new List<GrainMonitoringInformation>();
                informations.Add(interfaceId, interfaceInformations);
            }
            interfaceInformations.Add(information);
            return Task.CompletedTask;
        }

        public Task<IList<GrainMonitoringInformation>> GetInformation(int interfaceId)
        {
            List<GrainMonitoringInformation> interfaceInformations;
            if(informations.TryGetValue(interfaceId, out interfaceInformations))
            {
                return Task.FromResult<IList<GrainMonitoringInformation>>(interfaceInformations);
            }
            else
            {
                return Task.FromResult<IList<GrainMonitoringInformation>>(blankInformation);
            }
        }

        private static readonly List<GrainMonitoringInformation> blankInformation = new List<GrainMonitoringInformation>();
        private Dictionary<int,List<GrainMonitoringInformation>> informations;
    }
}
