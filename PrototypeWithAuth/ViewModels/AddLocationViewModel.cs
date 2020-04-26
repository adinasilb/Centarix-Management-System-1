using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Models.LocationsTierInstantiation;

namespace PrototypeWithAuth.ViewModels
{
    public class AddLocationViewModel
    {
        
        public LocationsTier1Instance locationsTier1Instance { get; set; }
        public IEnumerable<LocationsTier1Model> locationsTier1Models { get; set; }
    }
}
