using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class RecurringOrderEndStatusData
    {
        public static List<RecurrenceEndStatus> Get()
        {
            List<RecurrenceEndStatus> list = new List<RecurrenceEndStatus>();
            list.Add(new RecurrenceEndStatus
            {
                ID = 1,
                Description = "No End",
                DescriptionEnum =AppUtility.RecurrenceEndStatuses.NoEnd.ToString()
            });
            list.Add(new RecurrenceEndStatus
            {
                ID = 2,
                Description = "End Date",
                DescriptionEnum = AppUtility.RecurrenceEndStatuses.EndDate.ToString()
            });
            list.Add(new RecurrenceEndStatus
            {
                ID = 3,
                Description = "Limited Occurrences",
                DescriptionEnum = AppUtility.RecurrenceEndStatuses.LimitedOccurrences.ToString()
            });
            return list;
        }
    }
}
