using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeHoursStatus
    {
        public int EmployeeHoursStatusID { get; set; }
        public string Description { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHours {get; set;}
        public IEnumerable<EmployeeHoursAwaitingApproval> EmployeeHoursAwaitingApprovals { get; set; }

    }
}
