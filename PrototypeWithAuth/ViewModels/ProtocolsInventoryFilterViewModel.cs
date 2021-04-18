using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;


namespace PrototypeWithAuth.ViewModels
{
    public class ProtocolsInventoryFilterViewModel
    {
        public List<Employee> Owners { get; set; }
        public List<Employee> SelectedOwners { get; set; }
        public List<ProtocolCategory> ProtocolCategories { get; set; }
        public List<ProtocolCategory> SelectedProtocolCategories { get; set; }
        public List<ProtocolSubCategory> ProtocolSubCategories { get; set; }
        public List<ProtocolSubCategory> SelectedProtocolSubCategories { get; set; }
        public int NumFilters { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
