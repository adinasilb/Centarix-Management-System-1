using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class ParentCategory : CategoryBase
    {
        //[Key]
        //public int ID { get; set; }

        //[Required]
        //[Display(Name = "Category")]
        //public string Description { get; set; }
        public string ParentCategoryDescriptionEnum { get
            {
                return Description?.Replace(" ", "");
            } }


        public ListImplementsModelBase<ProductSubcategory> ProductSubcategories { get; set; }
        // public IEnumerable<Vendor> Vendors { get; set; }

        public int CategoryTypeID { get; set; }
        public CategoryType CategoryType { get; set; }
        public bool IsProprietary { get; set; }
        public IEnumerable<UnitTypeParentCategory> UnitTypeParentCategory { get; set; }
        //public override CategoryJson CategoryJson { get; set; }
    }
}
