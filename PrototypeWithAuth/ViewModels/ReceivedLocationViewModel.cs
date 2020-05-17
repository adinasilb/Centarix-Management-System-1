using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ReceivedLocationViewModel
    {
        public ApplicationUser applicationUserHolder { get; set; }
        public int LocationTypeID { get; set; }//this is just here for now so we have a place to store the dropdownlistfor but I don't know if we actually needs to save it b/c we don't need to access it
        public IEnumerable<LocationType> locationTypesDepthZero { get; set; }
        public List<LocationInstance> locationInstancesSelected { get; set; }
        public Request Request { get; set; }

        //public List<SubLocationInstancesViewModel> subLocationInstancesViewModels {get; set;}
    }
}
