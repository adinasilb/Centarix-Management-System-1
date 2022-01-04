using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeHours :EmployeeHoursBase
    {
        [Key]
        public int EmployeeHoursID { get; set; } 
        public DateTime Date_submit { get { return Date; } set { Date = value; } }       
        public int? CompanyDayOffID { get; set; }
        public CompanyDayOff CompanyDayOff { get; set; }
        public EmployeeHoursAwaitingApproval EmployeeHoursAwaitingApproval { get;set;}
    }
}
