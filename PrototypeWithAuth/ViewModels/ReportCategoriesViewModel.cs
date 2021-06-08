using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReportCategoriesViewModel
    {
        public string PageType { get; set; }
        public string SidebarType { get; set; }
        public IEnumerable<ResourceCategory> ReportCategories { get; set; }
    }
}
