using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Shared.Placement;
using System.Collections.Generic;

namespace SiloCore.Placement
{
    public class Location : Grain<State.LocationState>, ILocation
    {
        public Task<SiloAddress> GetAddress()
        {
            return Task.FromResult(State.address);
        }

        public Task<SiloAddress> GetAddressAndSubscribe(ILocationSubscriber subscriber)
        {
            Subscribe(subscriber).Wait();
            return GetAddress();
        }

        public Task SetAddress(SiloAddress address)
        {
            if (State.address == address)
            {
                return Task.CompletedTask;
            }
            else
            {
                State.address = address;
                List<Task> tasks = new List<Task>();
                tasks.Add(WriteStateAsync());
                foreach(ILocationSubscriber subscriber in State.subscribers)
                {
                    tasks.Add(subscriber.Update(address, this));
                }
                return Task.WhenAll(tasks);
            }
        }

        public Task Subscribe(ILocationSubscriber subscriber)
        {
            if (!State.subscribers.Contains(subscriber))
            {
                State.subscribers.Add(subscriber);
                return WriteStateAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public Task Unsubscribe(ILocationSubscriber subscriber)
        {
            if(State.subscribers.Contains(subscriber))
            {
                State.subscribers.Remove(subscriber);
                return WriteStateAsync();
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}
