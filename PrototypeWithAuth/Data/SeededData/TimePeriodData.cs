using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class TimePeriodData
    {
        public static List<TimePeriod> Get()
        {
            List<TimePeriod> list = new List<TimePeriod>();
            list.Add(new TimePeriod
            {
                ID = 1,
                Description = "Days",
                DescriptionEnum = AppUtility.TimePeriods.Days.ToString()
            });
            list.Add(new TimePeriod
            {
                ID = 2,
                Description = "Weeks",
                DescriptionEnum = AppUtility.TimePeriods.Weeks.ToString()
            });
            list.Add(new TimePeriod
            {
                ID = 3,
                Description = "Months",
                DescriptionEnum = AppUtility.TimePeriods.Months.ToString()
            });

            return list;
        }
    }
}
