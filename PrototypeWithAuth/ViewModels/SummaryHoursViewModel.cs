﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class SummaryHoursViewModel
    {
        public List<EmployeeHoursAndAwaitingApprovalViewModel> EmployeeHours { get; set; }
        public DateTime CurrentMonth { get; set; }
        public int  SelectedYear { get; set;  }
        public double? TotalHoursInMonth { get; set; }
        public int TotalHolidaysInMonth { get; set; }
        public double VacationDayInThisMonth { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public Employee User { get; set; }
    }
}
