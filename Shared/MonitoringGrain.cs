using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using System;
using System.IO;
using System.Threading.Tasks;
using Orleans.Runtime;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Shared
{
    [Reentrant]
    public class MonitoringGrain : Grain, IMonitoring
    {
        private readonly ILogger logger;
        private static readonly TimeSpan DefaultTimerInterval = TimeSpan.FromSeconds(1);

        public MonitoringGrain(ILogger<MonitoringGrain> logger)
        {
            this.logger = logger;
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        private async Task<Task> CallbackWindows(object src)
        {

            var metricsGrain = GrainFactory.GetGrain<IManagementGrain>(0);
            var silos = await metricsGrain.GetDetailedHosts(true);
            Console.WriteLine("\nNumber of Silos : {0}\n", silos.Length);

            string newFileName = "test.csv";

            if (!File.Exists(newFileName))
            {
                string clientHeader = "Timestamp" + "," + "Silo ID" + "," + "CPU" + "," + "Memory Usage" + "," + "Sent Messages" + "," + "Recieved Messages" + Environment.NewLine;

                File.WriteAllText(newFileName, clientHeader);
            }

            foreach (var silo in silos)
            {
                var siloRuntimeStatistics = (await metricsGrain.GetRuntimeStatistics(new[] { silo.SiloAddress })).First();
                var siloId = silo.SiloAddress.ToString();
                var cpuUsage = siloRuntimeStatistics.CpuUsage.ToString();
                var memoryUsage = siloRuntimeStatistics.MemoryUsage.ToString();
                var sentMessages = siloRuntimeStatistics.SentMessages.ToString();
                var recievedMessages = siloRuntimeStatistics.ReceivedMessages.ToString();
                var timeStamp = siloRuntimeStatistics.DateTime.ToString();

                string details = timeStamp + "," + siloId + "," + cpuUsage + "," + memoryUsage + "," + sentMessages + "," + recievedMessages + Environment.NewLine;

                File.AppendAllText(newFileName, details);

            }
            return Task.CompletedTask;
        }

        private Task CallbackLinux(object src)
        {
            Process process = Process.Start(new ProcessStartInfo("bash", "test.sh")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine(output);

            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    RegisterTimer(CallbackLinux, null, DefaultTimerInterval, DefaultTimerInterval);
                }
                else
                {
                    RegisterTimer(CallbackWindows, null, DefaultTimerInterval, DefaultTimerInterval);
                }

            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Not running in Orleans runtime");
            }

            return base.OnActivateAsync();
        }
    }
}
