using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Shared.Placement
{
    public interface IPlacementHolderManager : IGrainWithGuidKey
    {
        Task<ICollection<IPlacement>> GetPlacementHolders();
        Task NewPlacementHolder(IPlacement holder);
    }
}
