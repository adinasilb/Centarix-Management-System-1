using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddCommentViewModel : ViewModelBase
    {
        public VendorComment VendorComment { get; set; }
        public int Index { get; set; }
        public bool IsEdit { get; set; }
    }
}
