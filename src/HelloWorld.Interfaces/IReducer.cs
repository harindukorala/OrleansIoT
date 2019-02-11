using System.Threading.Tasks;

namespace HelloWorld.Interfaces
{
    public interface IReducer : Orleans.IGrainWithIntegerKey
    {
        Task Reduce(Pairs subResults);
        Task<Pairs> GetResult();
        Task ReduceAccelo(AcceloDataPerSec subResults);
        Task<AcceloDataPerSec> GetResultAccelo();
    }
}
