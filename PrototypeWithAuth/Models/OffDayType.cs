﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class OffDayType : ModelBase
    {
        [Key]
        public int OffDayTypeID { get; set; }
        public string Description { get; set; }
        public string DescriptionEnum { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHoursPartial { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHours { get; set; }
        public IEnumerable<EmployeeHoursAwaitingApproval> EmployeeHoursAwaitingApprovalsPartial { get; set; }
        public IEnumerable<EmployeeHoursAwaitingApproval> EmployeeHoursAwaitingApprovals { get; set; }
    }
}
