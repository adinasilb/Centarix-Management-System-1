using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeHoursAwaitingApproval:EmployeeHoursBase
    {
        [Key]
        public int EmployeeHoursAwaitingApprovalID { get; set; }
        public int EmployeeHoursID { get; set; }
        public EmployeeHours EmployeeHours { get; set; }
        [DefaultValue(false)]
        public bool IsDenied { get; set; }
    }
}