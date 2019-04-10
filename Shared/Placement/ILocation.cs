using Orleans;
using System;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Shared.Placement 
{
    public interface ILocation : IGrainWithGuidKey
    {
        Task<SiloAddress> GetAddress();
        Task<SiloAddress> GetAddressAndSubscribe(ILocationSubscriber subscriber);
        Task SetAddress(SiloAddress address);
        Task Subscribe(ILocationSubscriber subscriber);
        Task Unsubscribe(ILocationSubscriber subscriber);
    }
}
