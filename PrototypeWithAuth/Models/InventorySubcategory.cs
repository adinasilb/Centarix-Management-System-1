using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.Models
{
    public class InventorySubcategory
    {
        [Key]
        public int InventorySubcategoryID { get; set; }
        
        [Required]
        [Display(Name = "Item Name")]
        public string InventorySubcategoryDescription { get; set; }

        
        public ICollection<Inventory> Inventories { get; set; }

    }
}
