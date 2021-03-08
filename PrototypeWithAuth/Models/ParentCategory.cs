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
        [Display(Name = "Category")]
        public string ParentCategoryDescription { get; set; }
        public string ParentCategoryDescriptionEnum { get
            {
                return ParentCategoryDescription?.Replace(" ", "");
            } }


        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        // public IEnumerable<Vendor> Vendors { get; set; }

        public int CategoryTypeID { get; set; }
        public CategoryType CategoryType { get; set; }
        public bool isProprietary { get; set; }
        public IEnumerable<UnitTypeParentCategory> UnitTypeParentCategory { get; set; }
    }
}
