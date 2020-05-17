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

namespace PrototypeWithAuth.ViewModels
{
    //this is the model for the modal view - this views a specific request and details - CRU (D)
    public class RequestItemViewModel
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<RequestStatus> RequestStatuses { get; set; }
        public IEnumerable<SelectListItem> UnitTypeList { get; set; }
        public List<string> OrderFileStrings { get; set; }

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
        public IEnumerable<Payment> OldPayments { get; set; }
        public List<Payment> NewPayments { get; set; }
        public IEnumerable<PaymentType> PaymentTypes { get; set; }
        public IEnumerable<CompanyAccount> CompanyAccounts { get; set; }
        public Request Request { get; set; } // requests already include the product, we do not need to include a separate product

        public LocationInstance ParentLocationInstance { get; set; } //DO WE NEED THIS?????????
        public List<LocationInstance> ChildrenLocationInstances { get; set; } //need this in a list b/c we need to 



        //The following properties are for remembering where you are on the request Index to follow through to the right page
        public int? Page { get; set; }
        public int RequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int VendorID { get; set; }
        public string ApplicationUserID { get; set; }
        public int /*AppUtility.RequestPageTypeEnum*/ PageType { get; set; }
    }
}
