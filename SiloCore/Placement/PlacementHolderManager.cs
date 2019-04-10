using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Shared;
using Shared.Placement;

namespace SiloCore.Placement
{
    class PlacementHolderManager : Grain<HashSet<IPlacement>>, IPlacementHolderManager
    {
        public Task<ICollection<IPlacement>> GetPlacementHolders()
        {
            return Task.FromResult<ICollection<IPlacement>>(State);
        }

        public Task NewPlacementHolder(IPlacement holder)
        {
            if (!State.Contains(holder))
            {
                State.Add(holder);
                return WriteStateAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
            
        }
    }
}
