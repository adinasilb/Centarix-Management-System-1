using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;
using MimeKit.Cryptography;

namespace PrototypeWithAuth.Models
{
    public class Request //WHEN YOU RETURN REQUEST MAY WANT TO DO A SELECT SO IT DOESN"T ALWAYS NEED TO CALCULATE THE DATE PAID
    {
        //IMPT: When adding in data validation make sure that you turn data-val off in the search
        [Key]
        public int RequestID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int? ParentRequestID { get; set; }
        public ParentRequest ParentRequest { get; set; }
        public int? SubProjectID { get; set; }
        public SubProject SubProject { get; set; }
        public IEnumerable<RequestLocationInstance> RequestLocationInstances { get; set; } //a request can go to many locations
        public int? RequestStatusID { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string ApplicationUserCreatorID { get; set; }
        [ForeignKey("ApplicationUserCreatorID")]
        public ApplicationUser ApplicationUserCreator { get; set; }
        public string ApplicationUserReceiverID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserReceiverID")]
        public ApplicationUser ApplicationUserReceiver { get; set; }
        public uint? AmountWithInLocation { get; set; } // in order to place different request objects into a location in a box, only dependent on the largest unit
        public uint? AmountWithOutLocation { get; set; } // in order to place different request objects into a location in a box

        public bool IsDeleted { get; set; } // check if this request's parentrequests requests are deleted - if so give parent request true for is deleted
        public bool IsApproved { get; set; }
      //  [Range(1, uint.MaxValue, ErrorMessage = "Field must be more than 0")]
        public uint Unit { get; set; } //largest unit the request comes in - amount of unit
        //public int Unit { get; set; }
        public int? UnitTypeID { get; set; }
        [ForeignKey("UnitTypeID")]
        [Display(Name = "Unit Type")]

        public UnitType UnitType { get; set; }
        // public int[] UnitTypes { get; set; }
        [Display(Name = "Subunit")]
        public uint? SubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subunit
        public int? SubUnitTypeID { get; set; }
        [ForeignKey("SubUnitTypeID")]
        [Display(Name = "Subunit Type")]
        public UnitType SubUnitType { get; set; }
        [Display(Name = "Sub Subunit")]
        public uint? SubSubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subsubunit

        public int? SubSubUnitTypeID { get; set; }

        [ForeignKey("SubSubUnitTypeID")]
        [Display(Name = "Sub Subunit Type")]
        public UnitType SubSubUnitType { get; set; }
        public uint UnitsOrdered { get; set; } //goes on whatever is the current smallest (if they add a smaller unit --> should be changed in the frontend)
        public uint UnitsInStock { get; set; } //goes on whatever is the current smallest (if they add a smaller unit --> should be changed in the frontend)
        public uint Quantity { get; set; }

        ///[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Range(1, Double.MaxValue, ErrorMessage = "Field must be more than 0")]
        [Display(Name = "Price")]
        [Required]
        public double Cost { get; set; } //this is always shekel no matter what currency is
        public string Currency { get; set; }

        [Range(1, 2147483647, ErrorMessage = "Field must be a number")]
        [Required]
        [Display(Name = "Catalog Number")]
        public string CatalogNumber { get; set; }
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Display(Name = "Website")]
        public string URL { get; set; }
        //public string ExpenseDescription { get; set; } - this is really the product name - so when view products, only return those with a specific subcategoryID,

        [Range(0, 255, ErrorMessage = "Field must be positive")]
        public byte? Warranty { get; set; } // this is the amount of months of the warranty. the datetime when it ends will be calculated on the frontend (now it's from the date ordered, but should it be from the date received instead?)

        public double VAT { get; set; } // should this be coded into the model or should we set it elsewhere?
        [Display(Name = "Exchange Rate")]
        public double ExchangeRate { get; set; } // holding the rate of exchange for this specific request
        public int? Terms { get; set; } // if terms is selected, keep decremtnting, when = zero, gets status of pay now
        [Display(Name = "Expected Supply Days")]

        [Range(0, 2147483647, ErrorMessage = "Field must be a positive number")]
        public byte? ExpectedSupplyDays { get; set; } // will need to cast it to datetime when calulating the expected supply date, in the front end
        /*public string RequestComment { get; set; }*/ //can take this out - Adina

        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date")]
        public DateTime ArrivalDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public int? ParentQuoteID { get; set; }
        public ParentQuote ParentQuote { get; set; }

        public bool Paid { get; set; }
        public uint? Installments { get; set; } //number of installments
        //public IEnumerable<Payment> Payments { get; set; }

        public int? PaymentStatusID { get; set; }
        [ForeignKey("PaymentStatusID")]
        public PaymentStatus PaymentStatus { get; set; }

        public int? InvoiceID { get; set; }
        [ForeignKey("InvoiceID")]
        public Invoice Invoice { get; set; }




    }
}
