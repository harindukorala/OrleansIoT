using HelloWorld.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Grains
{
    [StatelessWorker]
    public class MapperTestGrain : Orleans.Grain, IMapper
    {
        //Implementation code 
        private readonly ILogger logger;

        public MapperTestGrain(ILogger<MapperTestGrain> logger)
        {
            this.logger = logger;
        }

        public Task<Pairs> MapAsync(string document)
        {
            var items = from w in document.Trim('"').Split(' ', ',', ':', '.', ';','"')
                        group 1 by w into g
                        select new Pair(g.Key, g.Sum());

            var newItem = from w in document.Trim('"').Split(',')
                    select new Pair(w, 1);

            var foo = (from w in document.Trim('"').Split(',')
                       select w).First();

            

            List < Pair > test = new List<Pair>
            {
                new Pair(foo, 1)
            };

            //var test = document.Split(' ', ',', ':', '.', ';');
            // var yu = test.First();
            //var items3 = from w in document
            //             group 1 by w into g
            //             select new Pair(g.Key.ToString(), g.Count());
            var result = new Pairs(test);
            return Task.FromResult(result);
        }

        public Task<AcceloDataPerSec> MapAccelo(string document)
        {
            string[] sada = document.Split(',');
            double x, y, z;
            x = double.Parse(sada[1]);
            y = double.Parse(sada[2]);
            z = double.Parse(sada[3]);

            //Calculate the Average for accelerometr X,Y,Z per one timestamp
            double average = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

            List<AcceloData> accelo = new List<AcceloData>
            {
                new AcceloData(sada[0], 1, average)
            };

            logger.LogInformation(average.ToString());

            var result = new AcceloDataPerSec(accelo);
            return Task.FromResult(result);

        }

        Task<string> IMapper.SayHello(string greeting)
        {
            logger.LogInformation($"SayHello message received: greeting = '{greeting}'");
            return Task.FromResult($"You said: '{greeting}', I say: Hello Mapper Bye!");
        }
    }
}
