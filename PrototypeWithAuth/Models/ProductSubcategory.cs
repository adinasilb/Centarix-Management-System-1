using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.Models
{
    public class ProductSubcategory : CategoryBase
    {
        //[Key]
        //public int ID { get; set; }
        [Required]
        public int ParentCategoryID { get; set; }
        public ParentCategory ParentCategory { get; set; }

        //[Required]
        //[Display(Name = "Subcategory")]
        //public string Description { get; set; }

        public IEnumerable<Product> Products { get; set; }
        public string ImageURL { get; set; }

        public bool IsOldSubCategory {get; set;}
    }
}
