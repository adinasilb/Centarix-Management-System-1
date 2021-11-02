using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ReportTypeData
    {
        public static List<ReportType> Get()
        {
            List<ReportType> list = new List<ReportType>();
            list.Add(new ReportType
            {
                ReportTypeID = 1,
                ReportTypeDescription = "Daily"
            });
            list.Add(new ReportType
            {
                ReportTypeID = 2,
                ReportTypeDescription = "Weekly"
            });
            list.Add(new ReportType
            {
                ReportTypeID = 3,
                ReportTypeDescription = "Monthly"
            });
            return list;
        }
    }
}