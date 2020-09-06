using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReceivedModalVisualViewModel
    {
        public LocationInstance ParentLocationInstance { get; set; }
        public List<LocationInstance> ChildrenLocationInstances { get; set; }
        public bool DeleteTable { get; set; } //set this option to true if the "select" option is selected and we don't want to show anything
        //public List<bool> CheckedLocations { get; set; }
    }
}
