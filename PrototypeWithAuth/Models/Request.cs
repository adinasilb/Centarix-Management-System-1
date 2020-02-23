
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        public int ProductID { get; set; }
        public int LocationID { get; set; } // if status is recieved; then LocationID is required (have to make a custom rule)
        public int RequestStatusID { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string ApplicationUserID { get; set; }
        
        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public int OrderNumber { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }
        public bool WithOrder { get; set; } 
        public string InvoiceNumber { get; set; }
        public string CatalogNumber { get; set; }
        public string SerialNumber { get; set; }
        public string URL { get; set; }
        //public string ExpenseDiscription { get; set; } - this is really the product name - so when view products, only return those with a specific subcategoryID,


    }
}
