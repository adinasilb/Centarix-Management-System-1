using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;
using MimeKit.Cryptography;
using PrototypeWithAuth.AppData;

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
        [Display(Name = "Amount")]
        public uint Unit { get; set; } //largest unit the request comes in - amount of unit
        //public int Unit { get; set; }
        public int? UnitTypeID { get; set; }
        [ForeignKey("UnitTypeID")]
        [Display(Name = "Unit")]

        public UnitType UnitType { get; set; }
        // public int[] UnitTypes { get; set; }
        [Display(Name = "Amount")]
        public uint? SubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subunit
        public int? SubUnitTypeID { get; set; }
        [ForeignKey("SubUnitTypeID")]
        [Display(Name = "Unit")]
        public UnitType SubUnitType { get; set; }
        [Display(Name = "Amount")]
        public uint? SubSubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subsubunit

        public int? SubSubUnitTypeID { get; set; }

        [ForeignKey("SubSubUnitTypeID")]
        [Display(Name = "Unit")]
        public UnitType SubSubUnitType { get; set; }
        //public uint UnitsOrdered { get; set; } //goes on whatever is the current smallest (if they add a smaller unit --> should be changed in the frontend)
        //public uint UnitsInStock { get; set; } //goes on whatever is the current smallest (if they add a smaller unit --> should be changed in the frontend)

        ///[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Range(1, Double.MaxValue, ErrorMessage = "Field must be more than 0")]
        [Display(Name = "Price")]
        [Required]
        public double Cost { get; set; } //this is always shekel no matter what currency is
        public string Currency { get; set; }

        [Required]
        [Display(Name = "Catalog Number")]
        public string CatalogNumber { get; set; }
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Display(Name = "Website")]
        public string URL { get; set; }
        //public string ExpenseDescription { get; set; } - this is really the product name - so when view products, only return those with a specific subcategoryID,

        [Range(0, 255, ErrorMessage = "Field must be positive")]
        [Display(Name = "Warranty (Months)")]
        public byte? Warranty { get; set; } // this is the amount of months of the warranty. the datetime when it ends will be calculated on the frontend (now it's from the date ordered, but should it be from the date received instead?)
        [Display(Name = "Batch/Lot")]
        public int? Batch { get; set; }
        [Display(Name = "Expiration Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BatchExpiration { get; set; }
        public int? Passage { get; set; }
        public int? Amount { get; set; }
        private double _VAT;
        public double VAT {
            get {
                return Math.Round(.17 * Cost,2);
            }
            private set { _VAT = value; }

        }

        private double _PricePerUnit;
        public double PricePerUnit
        {
            get
            {
                return Cost / Unit==0?1:Unit;
            }
            private set { _PricePerUnit = value; }

        }

        private double? _PricePerSubUnit;
        public double? PricePerSubUnit
        {
            get
            {
                return PricePerUnit/SubUnit;
            }
            private set { _PricePerSubUnit = value; }

        }

        private double? _PricePerSubSubUnit;
        public double? PricePerSubSubUnit
        {
            get
            {
                return PricePerSubUnit / SubSubUnit;
            }
            private set { _PricePerSubSubUnit = value; }

        }

        private double _TotalWithVat;
        public double TotalWithVat
        {
            get
            {
                return Math.Round(VAT + Cost,2);
            }
            private set { _TotalWithVat = value; }

        }

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
        public DateTime ArrivalDate_submit { get { return ArrivalDate; } set { ArrivalDate = value; } }
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

        public string? NoteToSupplier { get; set; }
        public IEnumerable<RequestNotification> RequestNotifications { get; set; }
        public AppUtility.OrderTypeEnum OrderType {get; set;}
        public double Discount { get; set; }


    }
}
