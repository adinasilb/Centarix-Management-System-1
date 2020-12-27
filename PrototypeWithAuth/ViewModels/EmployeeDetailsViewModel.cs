using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public int FreelancerCount { get; set; }
        public int AdvisorCount { get; set; }
        public int SalariedEmployeeCount { get; set; }
        public IEnumerable<UserWithCentarixIDViewModel> Employees {get;set;}
    }
}
