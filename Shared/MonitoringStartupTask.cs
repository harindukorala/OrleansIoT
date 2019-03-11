using System;
using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;

namespace Shared
{
    public class MonitoringStartupTask : IStartupTask
    {
        private readonly IGrainFactory grainFactory;

        public MonitoringStartupTask(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var grain = this.grainFactory.GetGrain<IMonitoring>(0);
            await grain.Init();
        }
    }
}
