using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class Product
    {
        //IMPT: When adding in data validation make sure that you turn data-val off in the search
        //Rollback favorites and shares

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

        public int? UnitTypeID { get; set; }
        [ForeignKey("UnitTypeID")]
        [Display(Name = "Unit")]

        public UnitType UnitType { get; set; }
        // public int[] UnitTypes { get; set; }
        [Display(Name = "Amount")]
        [Range(0, double.MaxValue)]
        public decimal? SubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subunit
        public int? SubUnitTypeID { get; set; }
        [ForeignKey("SubUnitTypeID")]
        [Display(Name = "Unit")]
        public UnitType SubUnitType { get; set; }
        [Display(Name = "Amount")]
        [Range(0, Double.MaxValue)]
        public decimal? SubSubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subsubunit

        public int? SubSubUnitTypeID { get; set; }

        [ForeignKey("SubSubUnitTypeID")]
        [Display(Name = "Unit")]
        public UnitType SubSubUnitType { get; set; }

        public string ProductComment { get; set; }
        
        [Display(Name = "Image")]
        public string ProductMedia { get; set; }

        [Display(Name = "Catalog Number")]
        public string CatalogNumber { get; set; }
        public string ProductHebrewName { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        public DateTime ProductCreationDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
