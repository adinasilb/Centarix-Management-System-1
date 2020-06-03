using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class LocationInventoryIndexViewModel
    {
        public IEnumerable<LocationInstance> LocationsDepthOfZero { get; set; }
        public Dictionary<int, int> ItemsWithinLocation { get; set; }
    }
}
