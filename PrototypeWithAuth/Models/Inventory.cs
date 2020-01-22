using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

        [DataType(DataType.Date)]
        public DateTime ItemReceivedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ItemPaymentDate { get; set; }

       
        public InventorySubcategory InventorySubcategory { get; set; }

        public int InventorySubcategoryID { get; set; }
    }
}
