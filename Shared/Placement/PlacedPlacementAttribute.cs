using Orleans.Placement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class PlacedPlacementAttribute : PlacementAttribute
    {
        public PlacedPlacementAttribute() :
            base(PlacedPlacement.Singleton)
        {

        }
    }
}
