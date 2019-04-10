using Orleans;
using System;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Shared.Placement
{
    public interface IPlaced : IGrainWithGuidKey
    {
        Task Kill();
    }
}
