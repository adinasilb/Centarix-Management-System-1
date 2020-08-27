using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class LocationIndexViewModel
    {
        //public int LocationTypeParentID { get; set; }
        public IEnumerable<LocationInstance> LocationsDepthOfZero { get; set; }
        public IEnumerable<LocationInstance> SubLocationInstances { get; set; }
    }
}
