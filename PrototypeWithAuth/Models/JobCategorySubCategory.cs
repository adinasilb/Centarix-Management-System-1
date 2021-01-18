using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class JobCategorySubCategory
    {
        [Key, Column(Order = 1)]
        public int JobSubcategoryTypeID { get; set; }
        public JobSubcategoryType JobSubcategoryType { get; set; }
        [Key, Column(Order = 2)]
        public int JobCategoryTypeID { get; set; }
        public JobCategoryType JobCategoryType { get; set; }
    }
}
