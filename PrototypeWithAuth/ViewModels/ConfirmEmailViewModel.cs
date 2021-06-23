using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ConfirmEmailViewModel : ViewModelBase
    {
        public ParentRequest ParentRequest { get; set; }
        public List<Request> Requests { get; set; }
        public bool IsResend { get; set; }
        public int VendorId { get; set; }
        public int RequestID { get; set; }
        //public RequestIndexObject RequestIndexObject { get; set; }
        public TempRequestListViewModel TempRequestListViewModel { get; set; }
    }
}
