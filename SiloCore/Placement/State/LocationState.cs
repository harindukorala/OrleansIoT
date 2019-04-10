using System.Collections.Generic;
using Orleans.Runtime;
using Shared.Placement;

namespace SiloCore.Placement.State
{
    public class LocationState
    {
        public SiloAddress address = null;
        public HashSet<ILocationSubscriber> subscribers = new HashSet<ILocationSubscriber>();
    }
}
