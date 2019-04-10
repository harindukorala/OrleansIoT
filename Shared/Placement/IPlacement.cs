using Orleans;
using System;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Shared.Placement
{
    public interface IPlacement : IGrainWithGuidKey
    {
        Task<ILocation> GetLocation();
        Task SetLocation(ILocation location);
    }
}
