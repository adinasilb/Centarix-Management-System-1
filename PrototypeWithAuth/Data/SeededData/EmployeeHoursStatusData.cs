using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class EmployeeHoursStatusData
    {
        public static List<EmployeeHoursStatus> Get()
        {
            List<EmployeeHoursStatus> list = new List<EmployeeHoursStatus>();
            list.Add(new EmployeeHoursStatus
            {
                EmployeeHoursStatusID = 1,
                Description = "Work from home"
            });
            list.Add(new EmployeeHoursStatus
            {
                EmployeeHoursStatusID = 2,
                Description = "Edit existing hours"
            });
            list.Add(new EmployeeHoursStatus
              {
                  EmployeeHoursStatusID = 3,
                  Description = "Forgot to report"
              });
            return list;
        }
    }
}
