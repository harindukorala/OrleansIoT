using Orleans;
using Orleans.Runtime;
using Shared.Placement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiloCore.Placement
{
    public class Placement : Grain<State.PlacementState>, IPlacement, ILocationSubscriber
    {
        public Task<ILocation> GetLocation()
        {
            return Task.FromResult(State.location);
        }

        public Task SetLocation(ILocation location)
        {
            if(State.location != null)
            {
                State.location.Unsubscribe(this);
            }

            State.location = location;
            Task writeTask = WriteStateAsync();

            if(State.location == null)
            {
                cachedAddress = null;
            }
            else
            {
                SiloAddress newAddress = State.location.GetAddressAndSubscribe(this).Result;
                if(newAddress != cachedAddress)
                {
                    return Task.WhenAll(new Task[]{ writeTask, UpdateCachedAddress(newAddress)});
                }
            }
            return writeTask;
        }

        public Task Update(SiloAddress address, ILocation location)
        {
            if(State == location)
            {
                return UpdateCachedAddress(address);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private SiloAddress cachedAddress = null;

        private Task UpdateCachedAddress(SiloAddress newAddress)
        {
            cachedAddress = newAddress;
            return KillExisting();
        }

        private Task KillExisting()
        {
            return GrainFactory.GetGrain<IPlaced>(this.GetPrimaryKey()).Kill();
        }
    }
}
