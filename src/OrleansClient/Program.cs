using HelloWorld.Interfaces;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Reflection;
using Orleans.Hosting;

namespace OrleansClient
{
    /// <summary>
    /// Orleans test silo client
    /// </summary>
    public class Program
    {
        const int initializeAttemptsBeforeFailing = 5;
        private static int attempt = 0;

        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                // Purpose of using using is to dispose the object as soon as it goes out of scope 
                using (var client = await StartClientWithRetries())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> StartClientWithRetries()
        {
            attempt = 0;
            IClusterClient client;
            //var invariant = "MySql.Data.MySqlClient";
            //const string connectionString = "server=127.0.0.1;uid=root;pwd=password;database=orleansclustering";
            //const string connectionString = "DefaultEndpointsProtocol=https;AccountName=myorleansgroupdiag;AccountKey=l9i7PGttN9Z0pBCbEX88cluItSsm0RMK8O22ZS6dLeCyS8wRWDF/7k9NIL0ZGIgE8Lz7oD7RygpR0rI1l7AJ9Q==;EndpointSuffix=core.windows.net";
            var gateways = new IPEndPoint[]
{
    new IPEndPoint(IPAddress.Parse("10.0.75.1"), 30000),
    //new IPEndPoint(IPAddress.Parse("10.2.9.56"), 30001),
};
        client = new ClientBuilder()
                .UseLocalhostClustering()
                //.UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                //.UseStaticClustering(gateways)
                //.UseAdoNetClustering(options => { options.Invariant = invariant; options.ConnectionString = connectionString; })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect(RetryFilter);
            Console.WriteLine("Client successfully connect to silo host");
            return client;
        }

        private static async Task<bool> RetryFilter(Exception exception)
        {
            if (exception.GetType() != typeof(SiloUnavailableException))
            {
                Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
                return false;
            }
            attempt++;
            Console.WriteLine($"Cluster client attempt {attempt} of {initializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
            if (attempt > initializeAttemptsBeforeFailing)
            {
                return false;
            }
            await Task.Delay(TimeSpan.FromSeconds(4));
            return true;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            //var friend = client.GetGrain<IHello>(0);
            //var newfriend = client.GetGrain<IMapper>(0);
           // var response = await friend.SayHello("Good morning, my friend!");
            // var response = await newfriend.SayHello("Good morning, Mapper!");
            //var response = await newfriend.MapAsync("Good, Morning, Good, Morning, Yes, Good, Now, No");

            //Console.WriteLine("\n\n{0}\n\n", response.ToString());

            var reducerActor = client.GetGrain<IReducer>(0);
            List<Task> forks = new List<Task>();
            string fullDataSet = "Good,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes\nGood,Now,No,Good,Morning,Good,Morning\nYes,Good,Now,No,Good,Morning,Good,Morning,Yes\nGood,Now,No,Good,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No\nGood,Morning,Good,Morning,Yes,Good,Now,No";
            string[] words = fullDataSet.Split('\n');
            List<string> myData = new List<string>();

            //var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"Resources\TestData.csv");
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"Resources\AccTestData.csv");

            using (StreamReader fileContent = new StreamReader(fileName))
            {
                string headerLine = fileContent.ReadLine();
                string line;
                while ((line = fileContent.ReadLine()) != null)
                {
                    myData.Add(line);
                }
            }

            Console.WriteLine("Input Data Line by Line\n");
            Console.WriteLine(fileName);
            Console.WriteLine(myData.ToString());
            foreach (var t in myData.ToArray())
            {
                Console.WriteLine("\n{0}\n",t);
            }

            foreach(var w in myData.ToArray())
            {
                Task subProcess = ProcessSubResultAsync(client, reducerActor, w);
                forks.Add(subProcess);
            }

            await Task.WhenAll(forks);
            //var results = await reducerActor.GetResult();
            var results = await reducerActor.GetResultAccelo();
            Console.WriteLine("Reducer output : Aggregated Values\n");
            Console.WriteLine("\n{0}\n", results.ToString());

            //var friend = client.GetGrain<IHello>(0);
            //while (true)
            //{
            //    var response = await friend.SayHello("Good morning, my friend!");

            //    Console.WriteLine("\n\n{0}\n\n", response.ToString());

            //    await Task.Delay(TimeSpan.FromSeconds(5));
            //}

        }

        private static async Task ProcessSubResultAsync(
            IClusterClient client,
            IReducer proxy,
            string partialData)
        {
            var mapper = client.GetGrain<IMapper>(Guid.NewGuid());
            //Pairs subResult = await mapper.MapAsync(partialData);
            AcceloDataPerSec subResult = await mapper.MapAccelo(partialData);
            Console.WriteLine("\n\n{0}\n\n", subResult.ToString());
            //await proxy.Reduce(subResult);
            await proxy.ReduceAccelo(subResult);
        }
    }
}
