using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PrototypeWithAuth.ViewModels
{
    //this is the model for the modal view - this views a specific request and details - CRU (D)
    public class RequestItemViewModel
    {
        //public ApplicationUser CurrentUser { get; set; }
        public Request Request { get; set; } // requests already include the product, we do not need to include a separate product
        public List<Request> RequestsByProduct { get; set; }
        public ParentCategory ParentCategory { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<RequestStatus> RequestStatuses { get; set; }
        public IEnumerable<SelectListItem> UnitTypeList { get; set; }
        public ILookup<UnitParentType, UnitType> UnitTypes { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<SubProject> SubProjects { get; set; }
        public bool Paid { get; set; }
        public bool PayNow { get; set; }
        public bool PayLater { get; set; }
        public List<string> OrderFileStrings { get; set; }
        public List<string> InvoiceFileStrings { get; set; }
        public List<string> ShipmentFileStrings { get; set; }
        public List<string> QuoteFileStrings { get; set; }
        public List<string> InfoFileStrings { get; set; }
        public List<string> PictureFileStrings { get; set; }
        public List<string> ReturnFileStrings { get; set; }
        public List<string> CreditFileStrings { get; set; }

        //The PDFs that are passed into the controller:
        public List<IFormFile> OrderFiles { get; set; } //this needs to be changed b/c it is the pdf created by the order
        public List<IFormFile> InvoiceFiles { get; set; }
        public List<IFormFile> ShipmentFiles { get; set; }
        public List<IFormFile> QuoteFiles { get; set; }
        public List<IFormFile> InfoFiles { get; set; }
        public List<IFormFile> PictureFiles { get; set; }
        public List<IFormFile> ReturnFiles { get; set; }
        public List<IFormFile> CreditFiles { get; set; }


        public FileInfo[] OrderFilesFound { get; set; }
        public IEnumerable<Comment> OldComments { get; set; }
        public Comment NewComment { get; set; }

        public List<Payment> NewPayments { get; set; }
        public double Debt { get; set; } //shekel
        public IEnumerable<PaymentType> PaymentTypes { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Dictionary<int, IEnumerable<CompanyAccount>> CompanyAccountListsByPaymentTypeID { get; set; }
        public IEnumerable<CompanyAccount> CompanyAccounts { get; set; }

        public List<List<LocationInstance>> ChildrenLocationInstances { get; set; } 
        public LocationType LocationType { get; set; }
        public List<LocationInstance> LocationInstances { get; set; }

        public List<string> EmailAddresses { get; set; } //to pass back the email addresses in the create modal view
        public int RequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int VendorID { get; set; }
        public string ApplicationUserID { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public ReceivedLocationViewModel ReceivedLocationViewModel { get; set; }
        public ReceivedModalSublocationsViewModel ReceivedModalSublocationsViewModel { get; set; }
        public ReceivedModalVisualViewModel ReceivedModalVisualViewModel { get; set; }
        public LocationType ParentDepthZeroOfSelected { get; set; }
        public int Tab { get; set; }
        public List<AppUtility.CommentTypeEnum> CommentTypes { get; set; }
        public List<Comment> Comments { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public AppUtility.RequestModalType ModalType { get; set; }
    }
}
