using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shared.Monitoring
{
    public interface IGrainMonitoringHolder : IGrainWithGuidKey
    {
        Task AddInformation(int interfaceId, GrainMonitoringInformation information);
        Task<IList<GrainMonitoringInformation>> GetInformation(int interfaceId);
    }
}
