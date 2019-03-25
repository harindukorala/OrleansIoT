using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shared.Monitoring
{
    public interface ISiloMonitoringHolder : IGrainWithStringKey
    {
        Task<IList<SiloMonitoringInformation>> GetInformation();
        Task AddInformation(SiloMonitoringInformation information);
    }
}
