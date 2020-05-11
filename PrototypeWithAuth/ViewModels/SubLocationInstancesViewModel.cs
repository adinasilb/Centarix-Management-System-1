using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SubLocationInstancesViewModel
    {
        public List<LocationInstance> ListLocationInstances { get; set; }
        public string LocationTypeName { get; set; } //this is just so that its clear to the user what to select in the front end
        public int locationInstanceSelected { get; set; }
    }
}
