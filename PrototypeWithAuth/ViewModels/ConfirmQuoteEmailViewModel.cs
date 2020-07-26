using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmQuoteEmailViewModel
    {
        public IEnumerable<Request> Requests { get; set; }

        //The following properties are for remembering where you are on the request Index to follow through to the right page
        public int? Page { get; set; }
        public int RequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }
        public string ApplicationUserID { get; set; }
        public int /*AppUtility.RequestPageTypeEnum*/ PageType { get; set; }
    }
}
