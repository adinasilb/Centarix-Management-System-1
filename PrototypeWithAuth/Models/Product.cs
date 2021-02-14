using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.Models
{
    public class Product
    {
        //IMPT: When adding in data validation make sure that you turn data-val off in the search
        [Key]
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Item")]
        public string ProductName { get; set; }

        public int? VendorID { get; set; }
    
        public Vendor Vendor { get; set; }

        public int ProductSubcategoryID { get; set; }
        
        //[Required]
        
        public ProductSubcategory ProductSubcategory { get; set; }
        public IEnumerable<Request> Requests { get; set; }
        public string Handeling { get; set; }


        public int? QuantityPerUnit { get; set; }

        public int? UnitsInStock { get; set; }

        public int? UnitsInOrder { get; set; }

        public int ?ReorderLevel { get; set; }

        public string ProductComment { get; set; }
        
        [Display(Name = "Image")]
        public string ProductMedia { get; set; }

    }
}
