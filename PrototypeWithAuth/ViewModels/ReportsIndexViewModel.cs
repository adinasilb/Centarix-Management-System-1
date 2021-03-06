using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.ViewModels
{
    public class ReportsIndexViewModel : ViewModelBase
    {
        public IPagedList<ReportIndexPartialRowViewModel> PagedList { get; set; }
        
        public ReportsIndexObject ReportsIndexObject { get; set; }
        public bool CurrentReportCreated { get; set; }
    }
}
