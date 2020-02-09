using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.Models
{
    public class ProductSubcategory
    {
        [Key]
        public int ProductSubcategoryID { get; set; }
        [Required]
        public int ParentCategoryID { get; set; }
        public ParentCategory ParentCategory { get; set; }
        
        [Required]
        [Display(Name = "Type")]
        public string ProductSubcategoryDescription { get; set; }
    }
}
