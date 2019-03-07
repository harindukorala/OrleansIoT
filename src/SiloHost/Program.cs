using System;
using System.Net;
using System.Threading.Tasks;
using HelloWorld.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.Statistics;
using OrleansTelemetryConsumers.Counters;
using SiloCore;

namespace OrleansSiloHost
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .UseDashboard(options => { })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev"; // Unique ID for Orleans CLuster . All clients and Silos that use this ID will be able to talk directly to eachother 
                    options.ServiceId = "HelloWorldApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(MapperTestGrain).Assembly).WithReferences())
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Reducer).Assembly).WithReferences())
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(PlacementHolder).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .UsePerfCounterEnvironmentStatistics()
                                    .ConfigureServices(services =>
                                    {
                                        // Workaround for https://github.com/dotnet/orleans/issues/4129
                                        services.AddSingleton(cp => cp.GetRequiredService<IHostEnvironmentStatistics>() as ILifecycleParticipant<ISiloLifecycle>);
                                    })
                .AddMemoryGrainStorageAsDefault();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
