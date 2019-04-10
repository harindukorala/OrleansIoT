using System;
using Orleans.Runtime.Placement;
using Orleans;
using System.Threading.Tasks;
using Orleans.Runtime;
using Shared.Placement;

namespace SiloCore.Placement
{
    public class PlacedPlacementDirector : IPlacementDirector
    {
        public PlacedPlacementDirector(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }
        public async Task<SiloAddress> OnAddActivation(PlacementStrategy strategy, PlacementTarget target, IPlacementContext context)
        {
            Task<SiloAddress> fromHolder = grainFactory.GetGrain<IPlacement>(target.GrainIdentity.PrimaryKey).GetInterfaceSilo(
                target.GrainIdentity.TypeCode);


            var compatibleSilos = context.GetCompatibleSilos(target);
            Random random = new Random();
            SiloAddress randomSilo = compatibleSilos[random.Next(compatibleSilos.Count)];

            SiloAddress holderSilo = await fromHolder;

            if (holderSilo == null)
            {
                return randomSilo;
            }
            else
            {
                return holderSilo;
            }
        }


        private IGrainFactory grainFactory;
    }
}
