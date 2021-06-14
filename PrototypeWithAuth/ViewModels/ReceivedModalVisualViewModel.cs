using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReceivedModalVisualViewModel : ViewModelBase
    {
        public LocationInstance ParentLocationInstance { get; set; }
        public List<LocationInstance> ChildrenLocationInstances { get; set; }
        public List<RequestChildrenLocationInstances> RequestChildrenLocationInstances { get; set; } //only for the edit modal view so we can check if hasitems- if it's there
        public List<LocationInstancePlace> LocationInstancePlaces { get; set; }
        public bool DeleteTable { get; set; } //set this option to true if the "select" option is selected and we don't want to show anything
        public bool IsEditModalTable { get; set; }
        public bool ShowIcons { get; set; }
        //public List<bool> CheckedLocations { get; set; }
    }
}
