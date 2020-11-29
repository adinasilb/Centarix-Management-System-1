using Org.BouncyCastle.Asn1.Mozilla;
using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexViewModel
    {
       public IPagedList<PrototypeWithAuth.Models.Request> PagedList { get; set; }
       public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public List<String> PriceSortEnumsList { get; set; }
        public AppUtility.CurrencyEnum currency { get; set; }
        public int? Page { get; set; }
        public int RequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int VendorID { get; set; }
        public string ApplicationUserID { get; set; }
        public AppUtility.RequestPageTypeEnum PageType { get; set; }
        public AppUtility.MenuItems MenuType { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public int RequestParentLocationInstanceID { get; set; }

    }
}
