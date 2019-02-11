using HelloWorld.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld.Grains
{
    public class Reducer : Orleans.Grain<Pairs>, IReducer
    {
        Pairs state = new Pairs();
        AcceloDataPerSec newState = new AcceloDataPerSec();

        public Task<Pairs> GetResult()
        {
            return Task.FromResult(state);
        }

        public Task<AcceloDataPerSec> GetResultAccelo()
        {
            return Task.FromResult(newState);
        }

        public Task ReduceAccelo(AcceloDataPerSec subResults)
        {
            newState.MergeAccelo(subResults);
            return Task.FromResult(true);
        }

        public Task Reduce(Pairs subResults)
        {
            state.Merge(subResults);
            return Task.FromResult(true);
        }
    }
}
