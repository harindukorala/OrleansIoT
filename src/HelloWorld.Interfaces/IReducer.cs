using System.Threading.Tasks;

namespace HelloWorld.Interfaces
{
    public interface IReducer : Orleans.IGrainWithIntegerKey
    {
        Task ReduceAccelo(AcceloDataPerSec subResults);
        Task<AcceloDataPerSec> GetResultAccelo();
    }
}
