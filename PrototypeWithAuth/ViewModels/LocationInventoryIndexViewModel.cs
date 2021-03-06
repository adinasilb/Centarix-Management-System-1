using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class LocationInventoryIndexViewModel : ViewModelBase
    {
        public IEnumerable<LocationInstance> LocationsDepthOfZero { get; set; }
        public Dictionary<int, int> ItemsWithinLocation { get; set; }
    }
}
