using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Abstractions;

namespace PrototypeWithAuth.ViewModels
{
    public class AddNewItemViewModel
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<ProductSubcategory> ProductSubcategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<RequestStatus> RequestStatuses { get; set; }
        public Request Request { get; set; } // requests already include the product, we do not need to include a seperate product

        public HttpPostedFileBase OrderDoc { get; set; }
    }
}
