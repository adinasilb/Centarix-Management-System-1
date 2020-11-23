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
        public int? Page { get; set; }
        public int RequestStatusID { get; set; }
        public int SubCategoryID { get; set; }
        public int VendorID { get; set; }
        public string ApplicationUserID { get; set; }
        public int /*AppUtility.RequestPageTypeEnum*/ PageType { get; set; }
        public int RequestParentLocationInstanceID { get; set; }
    }
}
