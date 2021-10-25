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
    public class MoveListViewModel : ViewModelBase
    {
        public int PreviousListID { get; set; }
        public int NewListID { get; set; }
        public Request Request { get; set; }
        public List<RequestList> RequestLists { get; set; }
    }
}
