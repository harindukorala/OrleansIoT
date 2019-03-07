using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Shared
{
    public interface IPlacementHolder : IGrainWithGuidKey
    {
        Task<SiloAddress> GetSiloAddress(int interface_id);
        Task SetSiloAddress(int interface_id, SiloAddress address);
    }
}
