using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class SubLocationViewModel
    {
        public int LocationTypeParentID { get; set; }
        public IEnumerable<LocationType> LocationTypes { get; set; }
        public List<LocationInstance> LocationInstances { get; set; }

        public List<SelectListItem> BoxTypes { get; set; }
        public Dictionary<int, bool> EmptyShelves80 { get; set; }
        public bool ESTest { get; set; }
        public List<SelectListItem> EmptySelectList { get; set; }
    }
}
