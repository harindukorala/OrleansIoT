using HelloWorld.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Grains
{
    public class Reducer : Orleans.Grain<AcceloDataPerSec>, IReducer
    {
        AcceloDataPerSec newState = new AcceloDataPerSec();

        public Task<AcceloDataPerSec> GetResultAccelo()
        {
            return Task.FromResult(newState);
        }

        public Task ReduceAccelo(AcceloDataPerSec subResults)
        {
            newState.MergeAccelo(subResults);
            return Task.FromResult(true);
        }
    }
}
