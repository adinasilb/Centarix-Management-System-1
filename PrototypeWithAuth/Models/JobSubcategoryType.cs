using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class JobSubcategoryType
    {
        [Key]
        public int JobSubcategoryTypeID { get; set; }
        public string Description { get; set; }
        public int JobCategoryTypeID { get; set; }
        public JobCategoryType JobCategoryType { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
