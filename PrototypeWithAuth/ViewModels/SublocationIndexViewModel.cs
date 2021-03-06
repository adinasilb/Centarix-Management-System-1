using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class SublocationIndexViewModel : ViewModelBase
    {
        public IEnumerable<LocationInstance> SublocationInstances { get; set; }
        public LocationInstance PrevLocationInstance { get; set; }
        public bool IsSmallestChild { get; set; }

        public int Depth { get; set; }
    }
}
