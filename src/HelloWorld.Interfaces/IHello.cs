using Orleans;
using System.Threading.Tasks;

namespace HelloWorld.Interfaces
{
    /// <summary>
    /// Orleans grain communication interface IHello
    /// </summary>
    public interface IHello : IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}
