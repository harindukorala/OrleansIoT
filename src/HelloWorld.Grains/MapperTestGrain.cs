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
    }
}
