using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateReportViewModel : ViewModelBase
    {
        public Report Report { get; set; }
        public List<ReportSection> ReportSections { get; set; }
    }
}
