using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;


namespace PrototypeWithAuth.ViewModels
{
    public class InventoryFilterViewModel
    {
        //public List<CategoryType> Types { get; set; }
        //public List<CategoryType> SelectedTypes { get; set; }
        public List<Vendor> Vendors { get; set; }
        public List<Vendor> SelectedVendors { get; set; }
        public List<Employee> Owners { get; set; }
        public List<Employee> SelectedOwners { get; set; }
        public List<LocationType> Locations { get; set; }
        public List<LocationType> SelectedLocations { get; set; }
        public List<ParentCategory> Categories { get; set; }
        public List<ParentCategory> SelectedCategories { get; set; }
        public List<ProductSubcategory> Subcategories { get; set; }
        public List<ProductSubcategory> SelectedSubcategories { get; set; }
        public int NumFilters { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public bool Archive { get; set; }
        public bool IsProprietary { get; set; }
        //public List<Project> Projects { get; set; }
        //public List<int> SelectedProjects { get; set; }
        //public List<SubProject> SubProjects { get; set; }
        //public List<int> SelectedSubprojects { get; set; }
    }
}
