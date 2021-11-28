using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class OffDayTypeData
    {
        public static List<OffDayType> Get()
        {
            List<OffDayType> list = new List<OffDayType>();
            list.Add(new OffDayType
            {
                OffDayTypeID = 1,
                Description = "Sick Day"
            });
            list.Add(new OffDayType
                 {
                     OffDayTypeID = 2,
                     Description = "Vacation Day"
                 });
            list.Add(new OffDayType
                       {
                           OffDayTypeID = 3,
                           Description = "Maternity Leave"
                       });
            list.Add(new OffDayType
                   {
                       OffDayTypeID = 4,
                       Description = "Special Day"
                   });
            list.Add(new OffDayType
                   {
                       OffDayTypeID = 5,
                       Description = "Unpaid Leave"
                   });
            return list;
        }
    }
}
