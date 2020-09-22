using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class DeleteRequestViewModel
    {
        public Request Request { get; set; } 
        public bool IsReorder { get; set; }
        
        //The following properties are for remembering where you are on the request Index to follow through to the right page
        public int? Page { get; set; }
        public int RequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int VendorID { get; set; }
        public string ApplicationUserID { get; set; }
        public int /*AppUtility.RequestPageTypeEnum*/ PageType { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
    }
}
