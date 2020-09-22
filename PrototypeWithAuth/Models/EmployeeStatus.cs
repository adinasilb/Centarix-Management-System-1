using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeStatus 
    {
        [Key]
        public int EmployeeStatusID { get; set; }
        public string Description { get; set; }
        public IEnumerable<Employee> Employees {get; set;}
    }
}
