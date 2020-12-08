using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateSupplierViewModel
    {
        public Vendor Vendor { get; set; }
        public List<VendorContactWithDeleteViewModel> VendorContacts { get; set; }
        public List<VendorComment> VendorComments { get; set; }
        public List<PrototypeWithAuth.AppData.AppUtility.CommentTypeEnum> CommentTypes { get; set; }
        public AppUtility.MenuItems SectionType {get; set;}
        public List<CategoryType> CategoryTypes { get; set; }
        public List<int> VendorCategoryTypes { get; set; }
        public int Tab { get; set; }

    }
}
