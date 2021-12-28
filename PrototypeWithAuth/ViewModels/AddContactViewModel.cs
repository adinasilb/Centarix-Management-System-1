using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddContactViewModel : ViewModelBase
    {
        public VendorContact VendorContact{get; set;}
        public int Index { get; set; }
        public AppUtility.VendorModalType ModalType { get; set; }
    }
}
