using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ProjectStatistics
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; } //To be used for the _StatisticsProjects View
        public string SubProjectName { get; set; } //To be used for the _StatisticsSubProjects View
        public int Orders { get; set; }
        public int Items { get; set; }
        public int Price { get; set; }
    }
}
