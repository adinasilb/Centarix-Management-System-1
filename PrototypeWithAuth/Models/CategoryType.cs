using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CategoryType
    {
        [Key]
        public int CategoryTypeID { get; set; }
        public string CategoryTypeDescription { get; set; }

        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<VendorCategoryType> VendorCategoryTypes { get; set; }
    }
}
