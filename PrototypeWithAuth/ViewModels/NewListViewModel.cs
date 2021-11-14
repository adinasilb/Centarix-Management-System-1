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
    public class NewListViewModel : ViewModelBase
    {
        public string OwnerID { get; set; }
        public string ListTitle { get; set; }
        public int RequestToAddID { get; set; }
        public int RequestPreviousListID { get; set; }
    }
}
