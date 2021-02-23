using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ParentRequestWithPayByDateViewModel : ViewModelBase
    {
        public ParentRequest ParentRequest { get; set; }
        public DateTime? PayByDate { get; set; }
    }
}
