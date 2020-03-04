using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;

namespace PrototypeWithAuth.ViewModels
{
    //this is the model for the modal view - this views a specific request and details - CRU (D)
    public class RequestItemViewModel
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<RequestStatus> RequestStatuses { get; set; }
        public Request Request { get; set; } // requests already include the product, we do not need to include a seperate product

        public IList<IFormFile> OrderDocs { get; set; }
        public IList<IFormFile> InvoiceDocs { get; set; }
        public IList<IFormFile> ShipmentDocs { get; set; }
        public IList<IFormFile> QuoteDocs { get; set; }
        public IList<IFormFile> InfoDocs { get; set; }
        public IList<IFormFile> PicturesDocs { get; set; }
        public IList<IFormFile> ReturnDocs { get; set; }
        public IList<IFormFile> CreditDocs { get; set; }
    }
}
