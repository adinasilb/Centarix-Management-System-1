using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddWorkerViewModel : ViewModelBase
    {
        public List<SelectListItem> ApplicationUsers { get; set; }
        public Employee NewEmployee { get; set; }
        public List<JobCategoryType> JobCategoryTypes {get; set;}
        public List<EmployeeStatus> EmployeeStatuses { get; set; }
    }
}
