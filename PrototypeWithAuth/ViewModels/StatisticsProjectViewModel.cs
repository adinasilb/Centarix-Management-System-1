using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class StatisticsProjectViewModel
    {
        public Dictionary<Project, List<Request>> Projects { get; set; }
        public List<int> Months { get; set; }
        public int Year { get; set; }
    }
}
