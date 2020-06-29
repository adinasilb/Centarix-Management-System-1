using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;


namespace PrototypeWithAuth.ViewModels
{
    public class AddLocationViewModel
    {
        public IEnumerable<LocationType> LocationTypesDepthOfZero { get; set; }
        public LocationInstance LocationInstance { get; set; }
    }
}
