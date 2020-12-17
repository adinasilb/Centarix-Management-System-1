using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class VisualLocationsViewModel
    {
        public string Error { get; set; }
        public LocationInstance ParentLocationInstance { get; set; }
        public List<LocationInstance> ChildrenLocationInstances { get; set; }
        public bool IsSmallestChild { get; set; }
        public LocationInstance CurrentEmptyChild { get; set; }
        public AppUtility.MenuItems SectionType {get; set;}
    }
}
