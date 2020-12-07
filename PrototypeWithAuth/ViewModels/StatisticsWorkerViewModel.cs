using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class StatisticsWorkerViewModel
    {
        public Dictionary<Employee, List<Request>> Employees { get; set; }
        public List<CategoryType> CategoryTypesSelected { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public List<int> Months { get; set; }
        public List<int> Years { get; set; }
    }
}
