using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestsSearchViewModel : ViewModelBase
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<SubProject> SubProjects { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public Request Request { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        //booleans for request status
        public bool Inventory { get; set; }
        public bool Ordered { get; set; }
        public bool ForApproval { get; set; }
        public PrototypeWithAuth.AppData.AppUtility.MenuItems SectionType { get; set; }

        public IQueryable<Request> ReturnRequests { get; set; }
    }
}
