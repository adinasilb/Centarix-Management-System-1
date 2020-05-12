using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReceivedModalSublocationsViewModel
    {
        //the list of location instances that they selected (one for each select box)
        public List<LocationInstance> locationInstancesSelected { get; set; }

        //this will fill the first select box
        public IEnumerable<LocationInstance> locationInstancesDepthZero { get; set; }

        //use this to label the select boxes
        public List<string> locationTypeNames { get; set; }
        
    }
}
