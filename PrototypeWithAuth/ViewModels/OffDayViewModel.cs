using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class OffDayViewModel
    {
        public AppUtility.PageTypeEnum PageType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public AppUtility.OffDayTypeEnum OffDayType { get; set; }
        public int? Month { get; set; }
    }
}
