using PrototypeWithAuth.AppData;
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
                Description = "Sick Day",
                DescriptionEnum = AppUtility.OffDayTypeEnum.SickDay.ToString()
            });
            list.Add(new OffDayType
                 {
                     OffDayTypeID = 2,
                     Description = "Vacation Day",
                     DescriptionEnum = AppUtility.OffDayTypeEnum.VacationDay.ToString()
            });
            list.Add(new OffDayType
                       {
                           OffDayTypeID = 3,
                           Description = "Maternity Leave",
                           DescriptionEnum = AppUtility.OffDayTypeEnum.MaternityLeave.ToString()
            });
            list.Add(new OffDayType
                   {
                       OffDayTypeID = 4,
                       Description = "Special Day",
                       DescriptionEnum = AppUtility.OffDayTypeEnum.SpecialDay.ToString()
            });
            list.Add(new OffDayType
                   {
                       OffDayTypeID = 5,
                       Description = "Unpaid Leave",
                       DescriptionEnum = AppUtility.OffDayTypeEnum.UnpaidLeave.ToString()
            });
            return list;
        }
    }
}
