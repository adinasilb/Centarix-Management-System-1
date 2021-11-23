using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateSupplierViewModel : ViewModelBase
    {
        public Vendor Vendor { get; set; }
        public IEnumerable<VendorContactWithDeleteViewModel> VendorContacts { get; set; }
        public List<VendorComment> VendorComments { get; set; }
        public List<PrototypeWithAuth.AppData.AppUtility.CommentTypeEnum> CommentTypes { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public IEnumerable<CategoryType> CategoryTypes { get; set; }
        public List<int> VendorCategoryTypes { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public int Tab { get; set; }

    }
}
