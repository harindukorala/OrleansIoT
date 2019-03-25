using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    [Serializable]
    public class PlacedPlacement : PlacementStrategy
    {
        public static PlacedPlacement Singleton { get; } = new PlacedPlacement();
    }
}
