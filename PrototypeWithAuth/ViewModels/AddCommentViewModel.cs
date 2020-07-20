using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddCommentViewModel
    {
        public VendorComment VendorComment { get; set; }
        public bool IsActive { get; set; }
    }
}
