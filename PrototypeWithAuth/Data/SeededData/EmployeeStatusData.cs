using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class EmployeeStatusData
    {
        public static List<EmployeeStatus> Get()
        {
            List<EmployeeStatus> list = new List<EmployeeStatus>();
            list.Add(new EmployeeStatus
            {
                EmployeeStatusID = 1,
                Description = "Employee",
                Abbreviation = "E",
                LastCentarixID = 0
            });
            list.Add(new EmployeeStatus
            {
                EmployeeStatusID = 2,
                Description = "Freelancer",
                Abbreviation = "F",
                LastCentarixID = 0
            });
            list.Add(new EmployeeStatus
            {
                EmployeeStatusID = 3,
                Description = "Advisor",
                Abbreviation = "A",
                LastCentarixID = 0
            });
            list.Add(new EmployeeStatus
            {
                EmployeeStatusID = 4,
                Description = "User",
                Abbreviation = "U",
                LastCentarixID = 0
            });
            return list;
        }
    }
}
