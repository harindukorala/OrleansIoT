using Orleans;
using System;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Shared
{
    interface INode : IGrainWithGuidKey
    {
        Task Kill();
    }
}
