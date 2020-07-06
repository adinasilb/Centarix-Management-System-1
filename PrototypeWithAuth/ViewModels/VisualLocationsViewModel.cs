using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class VisualLocationsViewModel
    {
        public LocationInstance ParentLocationInstance { get; set; }
        public List<LocationInstance> ChildrenLocationInstances { get; set; }
        public bool IsSmallestChild { get; set; }
    }
}
