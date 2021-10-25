using Org.BouncyCastle.Asn1.Mozilla;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestListIndexViewModel : ViewModelBase
    {
        public IPagedList<Request> PagedList { get; set; }
        public int? Page { get; set; }
        public string ApplicationUserID { get; set; }
        public int ListID { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public AppUtility.MenuItems MenuType { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public List<RequestList> Lists { get; set; }
        public RequestIndexPartialViewModel RequestIndexPartialViewModel { get; set; }

    }
}
