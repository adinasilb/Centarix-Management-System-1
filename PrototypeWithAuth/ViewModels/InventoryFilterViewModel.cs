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
        public List<CategoryType> Types { get; set; }
        public List<int> SelectedTypes { get; set; }
        public List<Vendor> Vendors { get; set; }
        public List<int> SelectedVendors { get; set; }
        public List<Employee> Owners { get; set; }
        public List<string> SelectedOwners { get; set; }
        public List<LocationType> Locations { get; set; }
        public List<int> SelectedLocations { get; set; }
        public List<ParentCategory> Categories { get; set; }
        public List<int> SelectedCategories { get; set; }
        public List<ProductSubcategory> Subcategories { get; set; }
        public List<int> SelectedSubcategories { get; set; }
        public List<Project> Projects { get; set; }
        public List<int> SelectedProjects { get; set; }
        public List<SubProject> SubProjects { get; set; }
        public List<int> SelectedSubprojects { get; set; }
    }
}
