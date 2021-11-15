using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class CalibrationTypeData
    {
        public static List<CalibrationType> Get()
        {
            List<CalibrationType> list = new List<CalibrationType>();
            list.Add(new CalibrationType
            {
                CalibrationTypeID = 1,
                Description = "Repair",
                Icon = "icon-build-24px"
            });
            list.Add(new CalibrationType
            {
                CalibrationTypeID = 2,
                Description = "External Calibration",
                Icon = "icon-miscellaneous_services-24px-1"
            });
            list.Add(new CalibrationType
            {
                CalibrationTypeID = 3,
                Description = "In House Maintainance",
                Icon = "icon-inhouse-maintainance-24px"
            });
            return list;
        }
             
    }
}
