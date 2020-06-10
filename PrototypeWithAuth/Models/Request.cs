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
        public int ParentRequestID { get; set; }
        public ParentRequest ParentRequest { get; set; }
        public IEnumerable<RequestLocationInstance> RequestLocationInstances { get; set; } //a request can go to many locations
        public int? RequestStatusID { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string ApplicationUserID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUserReceiver { get; set; }
        public uint? AmountWithInLocation { get; set; } // in order to place different request objects into a location in a box, only dependent on the largest unit
        public uint? AmountWithOutLocation { get; set; } // in order to place different request objects into a location in a box

        public bool IsDeleted { get; set; } // check if this request's parentrequets requests are deleted - if so give parent request true for is deleted
        public bool IsApproved { get; set; }
        public uint Unit { get; set; } //largest unit the request comes in - amount of unit
        //public int Unit { get; set; }

        public int? UnitTypeID { get; set; }
        [ForeignKey("UnitTypeID")]
        public UnitType UnitType { get; set; }
        // public int[] UnitTypes { get; set; }
        public uint? SubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subunit
        public int? SubUnitTypeID { get; set; }
        [ForeignKey("SubUnitTypeID")]
        public UnitType SubUnitType { get; set; }
        public uint? SubSubUnit { get; set; } // if this is not null, then it this is the smallest unit - amount of subsubunit

        public int? SubSubUnitTypeID { get; set; }

        [ForeignKey("SubSubUnitTypeID")]
        public UnitType SubSubUnitType { get; set; }
        public uint UnitsOrdered { get; set; } //goes on whatever is the current smallest (if they add a smaller unit --> should be changed in the frontend)
        public uint UnitsInStock { get; set; } //goes on whatever is the current smallest (if they add a smaller unit --> should be changed in the frontend)
        public uint Quantity { get; set; }

        ///[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Price")]
        public double Cost { get; set; } //this is always shekel no matter what currency is
        public string Currency { get; set; }
        public string CatalogNumber { get; set; }
        public string SerialNumber { get; set; }
        public string URL { get; set; }
        //public string ExpenseDescription { get; set; } - this is really the product name - so when view products, only return those with a specific subcategoryID,

        public byte Warranty { get; set; } // will need to cast it to datetime when calulating the end date, in the front end

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)] //computed in modelbuilder
        //public DateTime WarrantyEndDate
        //{
        //    get
        //    {
        //        return this.ParentRequest.OrderDate.AddMonths(Warranty);
        //    }
        //    private set { }
        //}

        public double VAT = 0.17; // should this be coded into the model or should we set it elsewhere?
        public double ExchangeRate { get; set; } // holding the rate of exchange for this specic request
        public int? Terms { get; set; } // if terms is selected, keep decremtnting, when = zero, gets status of pay now
        public byte ExpectedSupplyDays { get; set; } // will need to cast it to datetime when calulating the expected supply date, in the front end
        public string RequestComment { get; set; } //can take this out - Adina

        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }



    }
}
