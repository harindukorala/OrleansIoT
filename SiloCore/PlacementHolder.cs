using Orleans;
using Orleans.Runtime;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiloCore
{
    public class PlacementHolder : Grain, IPlacementHolder
    {
        private Dictionary<int, SiloAddress> addresses = new Dictionary<int, SiloAddress>();

        public Task<SiloAddress> GetSiloAddress(int interface_id)
        {
            SiloAddress returning = null;
            if (addresses.TryGetValue(interface_id, out returning))
            {
                return Task.FromResult(returning);
            }
            else
            {
                return Task.FromResult(returning);
            }
        }

        //Grain methods should always return a Task
        public Task SetSiloAddress(int interface_id, SiloAddress address)
        {
            addresses[interface_id] = address;
            return Task.CompletedTask;
        }


    }
}
