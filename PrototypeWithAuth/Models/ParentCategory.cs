using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.Models
{
    public class ParentCategory
    {
        [Key]
        public int ParentCategoryID { get; set; }

        [Required]
        public string ParentCategoryDescription { get; set; }

       
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
    }
}
