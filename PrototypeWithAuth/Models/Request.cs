﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;
using MimeKit.Cryptography;
using PrototypeWithAuth.AppData;
using System.ComponentModel;

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
        public string ApplicationUserCreatorID { get; set; }
        [ForeignKey("ApplicationUserCreatorID")]
        public ApplicationUser ApplicationUserCreator { get; set; }
        public int? SubProjectID { get; set; }
        public SubProject SubProject { get; set; }

        private int _RequestStatusID { get; set; }
        public int RequestStatusID { 
            get
            {
                if (_RequestStatusID == 0)
                {
                    return 1;
                }
                else
                {
                    return _RequestStatusID;
                }
            }
            set { _RequestStatusID = value; } 
        }
        public RequestStatus RequestStatus { get; set; }

        public bool IsDeleted { get; set; } // check if this request's parentrequests requests are deleted - if so give parent request true for is deleted
        //  [Range(1, uint.MaxValue, ErrorMessage = "Field must be more than 0")]
        [Display(Name = "Amount")]
        public uint Unit { get; set; } //largest unit the request comes in - amount of unit

        [Display(Name = "URL")]
        public string URL { get; set; }
        [Range(0, 255, ErrorMessage = "Field must be positive")]
        [Display(Name = "Warranty (Months)")]
        public byte? Warranty { get; set; } // this is the amount of months of the warranty. the datetime when it ends will be calculated on the frontend (now it's from the date ordered, but should it be from the date received instead?)

        [Display(Name = "Batch/Lot")]
        public int? Batch { get; set; }
        [Display(Name = "Expiration Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BatchExpiration { get; set; }
        public DateTime? BatchExpiration_submit { get { return BatchExpiration; } set { BatchExpiration = value; } }
        //proprietary
        public int? Passage { get; set; }
        public int? Amount { get; set; }

        [Display(Name = "Expected Supply Days")]

        [Range(0, 2147483647, ErrorMessage = "Field must be a positive number")]
        public byte? ExpectedSupplyDays { get; set; } // will need to cast it to datetime when calulating the expected supply date, in the front end

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public int? ParentQuoteID { get; set; }
        public ParentQuote ParentQuote { get; set; }


        public string? NoteToSupplier { get; set; }
        public IEnumerable<RequestNotification> RequestNotifications { get; set; }
        public string OrderType { get; set; }

      


        //payment info
        public uint? Installments { get; set; } //number of installments
        public List<Payment> Payments { get; set; }

        public int? PaymentStatusID { get; set; }
        [ForeignKey("PaymentStatusID")]
        public PaymentStatus PaymentStatus { get; set; }
        public bool IsPartial { get; set; }
        public bool IsClarify { get; set; }
        public string NoteForClarifyDelivery { get; set; }
        
        //price info
        public string Currency { get; set; }
        [Range(1,  (double)Decimal.MaxValue, ErrorMessage = "Field must be more than 0")]
        [Display(Name = "Price")]
        public decimal? Cost { get; set; } //this is always shekel no matter what currency is
        private decimal _VAT;
        public decimal VAT
        {
            get
            {
                if (IncludeVAT == true)
                {
                    return Math.Round(.17m * Cost ?? 0, 2);
                }
                else
                {
                    return 0;
                }
            }
            private set { _VAT = value; }

        }

        private decimal _PricePerUnit;
        public decimal PricePerUnit
        {
            get
            {
                return (Cost ?? 0) / (Unit == 0 ? 1 : Unit);
            }
            private set { _PricePerUnit = value; }

        }

        //private decimal? _PricePerSubUnit;
        //public decimal? PricePerSubUnit
        //{
        //    get
        //    {
        //        return PricePerUnit / SubUnit;
        //    }
        //    private set { _PricePerSubUnit = value; }

        //}

        //private decimal? _PricePerSubSubUnit;
        //public decimal? PricePerSubSubUnit
        //{
        //    get
        //    {
        //        return PricePerSubUnit / SubSubUnit;
        //    }
        //    private set { _PricePerSubSubUnit = value; }

        //}

        private decimal _TotalWithVat;
        public decimal TotalWithVat
        {
            get
            {
                return Math.Round(VAT + Cost ?? 0, 2);
            }
            private set { ; }

        }

        [Display(Name = "Exchange Rate")]
        public decimal ExchangeRate { get; set; } // holding the rate of exchange for this specific request
        public int? Terms { get; set; } // if terms is selected, keep decremtnting, when = zero, gets status of pay now
        public decimal Discount { get; set; }



        /// received fields
        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date")]
        public DateTime ArrivalDate { get; set; }
        public DateTime ArrivalDate_submit { get { return ArrivalDate; } set { ArrivalDate = value; } }
        public string ApplicationUserReceiverID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserReceiverID")]
        public ApplicationUser ApplicationUserReceiver { get; set; }
        public IEnumerable<RequestLocationInstance> RequestLocationInstances { get; set; } //a request can go to many locations
        public bool Ignore { get; set; }
        public bool IsReceived { get; set; }
        public bool IncludeVAT { get; set; }

        public IEnumerable<ShareRequest> ShareRequests { get; set; }
        public IEnumerable<FavoriteRequest> FavoriteRequests { get; set; }
        public bool IsArchived { get; set; }
        public int? QuoteStatusID { get; set; }
        [ForeignKey("QuoteStatusID")]
        public QuoteStatus QuoteStatus { get; set; }
        public bool IsInInventory { get; set; }
    }
}
