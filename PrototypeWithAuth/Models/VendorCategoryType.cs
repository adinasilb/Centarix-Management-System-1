using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class VendorCategoryType
    {
        [Key, Column(Order = 1)]
        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }
        [Key, Column(Order = 2)]
        public int CategoryTypeID { get; set; }
        public CategoryType CategoryType { get; set; }
    }
}
