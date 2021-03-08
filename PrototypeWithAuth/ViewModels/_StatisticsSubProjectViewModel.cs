using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class _StatisticsSubProjectViewModel : ViewModelBase
    {
        public Dictionary<SubProject, List<Request>> SubProjects { get; set; }
        public string ProjectName { get; set; }
    }
}
