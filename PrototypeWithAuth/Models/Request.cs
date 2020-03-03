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
        public Product Product { get; set; }
        public int? LocationID { get; set; } // if status is recieved; then LocationID is required (have to make a custom rule)
        public int? RequestStatusID { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int? AmountWithInLocation { get; set; } // in order to place different request objects into a location in a box, only dependent on the largest unit
        public int? AmountWithOutLocation { get; set; } // in order to place different request objects into a location in a box
        public int Unit { get; set; } //largest unit the request comes in
        
        [ForeignKey("UnitTypeID")]
        public UnitType UnitType { get; set; }
        public int? SubUnit { get; set; } // if this is not null, then it this is the smallest unit
        
        [ForeignKey("SubUnitTypeID")]
        public UnitType SubUnitType { get; set; }
        public int? SubSubunit { get; set; } // if this is not null, then it this is the smallest unit
        
        [ForeignKey("SubSubUnitTypeID")]
        public UnitType SubSubUnitType { get; set; }
        public int UnitsOrdered { get; set; }
        public int UnitsInStock { get; set; }
        public string ApplicationUserID { get; set; }

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public int? OrderNumber { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }
        public bool WithOrder { get; set; }
        public string InvoiceNumber { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }
        public string CatalogNumber { get; set; }
        public string SerialNumber { get; set; }
        public string URL { get; set; }
        //public string ExpenseDescription { get; set; } - this is really the product name - so when view products, only return those with a specific subcategoryID,

        public byte Warranty { get; set; } // will need to cast it to datetime when calulating the end date, in the front end

        public byte ExpectedSupplyDays { get; set; } // will need to cast it to datetime when calulating the expected supply date, in the front end
        public string RequestComment { get; set; }

        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; } 

    }
}
