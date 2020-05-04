using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class SublocationIndexViewModel
    {
        public IEnumerable<LocationInstance> SublocationInstances { get; set; }
        public LocationInstance PrevLocationInstance { get; set; }
    }
}
