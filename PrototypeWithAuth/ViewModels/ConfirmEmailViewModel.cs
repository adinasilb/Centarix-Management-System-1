using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmEmailViewModel
    {
        public ParentRequest ParentRequest { get; set; }
        //public List<Request> Requests { get; set; }
        public int VendorId { get; set; }
        public int RequestID { get; set; }
        //public bool IsSingleOrder{ get; set; }

        //The following properties are for remembering where you are on the request Index to follow through to the right page
        public int? Page { get; set; }
        public int RequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }
        public string ApplicationUserID { get; set; }
        //public bool Cart { get; set; }
        public int /*AppUtility.RequestPageTypeEnum*/ PageType { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
