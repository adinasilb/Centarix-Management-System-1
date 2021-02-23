using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class StatisticsProjectViewModel : ViewModelBase
    {
        public Dictionary<Project, List<Request>> Projects { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
    }
}
