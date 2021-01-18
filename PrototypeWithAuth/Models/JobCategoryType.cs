using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class JobCategoryType
    {
        [Key]
        public int JobCategoryTypeID { get; set; }
        public string Description { get; set; }
        public IEnumerable<JobSubcategoryType> JobSubcategoryTypes { get; set; }
    }
}
