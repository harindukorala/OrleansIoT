using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace Shared
{
    public interface  IMonitoring : IGrainWithIntegerKey
    {
        [OneWay]
        Task Init();
    }
}
