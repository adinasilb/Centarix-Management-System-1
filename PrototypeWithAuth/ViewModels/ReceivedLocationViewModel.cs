using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
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
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }

        //booleans for the checkboxes
        public bool Arrival { get; set; }
        public bool Clarify { get; set; }
        public bool PartialDelivery { get; set; }

        //public List<SubLocationInstancesViewModel> subLocationInstancesViewModels {get; set;}

        //The following properties are for remembering where you are on the request Index to follow through to the right page
        public int? Page { get; set; }
        public int PageRequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int CategoryType { get; set; }
        public int VendorID { get; set; }
        public string ApplicationUserID { get; set; }
        public int /*AppUtility.RequestPageTypeEnum*/ PageType { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
