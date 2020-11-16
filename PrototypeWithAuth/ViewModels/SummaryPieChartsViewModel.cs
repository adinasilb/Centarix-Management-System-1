using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryPieChartsViewModel
    {
        public DateTime Year { get; set; }
        public List<int> Years { get; set; }
        public DateTime Month { get; set; }
        public int CategoryTypeID { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public int ParentCategoryID { get; set; }
        public List<ParentCategory> ParentCategories { get; set; }
        public int ProductSubcategoryID { get; set; }
        public List<ProductSubcategory> ProductSubcategories { get; set; }
        public int ProjectID { get; set; }
        public List<Project> Projects { get; set; }
        public int SubProjectID { get; set; }
        public List<SubProject> SubProjects { get; set; }
        public string AdvancedGraph { get; set; }
        public string Currency { get; set; }
        public int EmployeeID { get; set; }
        public List<SelectListItem> Employees { get; set; }
    }
}
