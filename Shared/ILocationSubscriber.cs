using Orleans;
using System;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Shared.Placement
{
    public interface ILocationSubscriber : IGrainWithGuidKey
    {
        Task Update(SiloAddress address, ILocation location);
    }
}
