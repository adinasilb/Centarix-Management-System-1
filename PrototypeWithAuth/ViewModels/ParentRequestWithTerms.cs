using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ParentRequestWithTerms : ViewModelBase
    {
        public ParentRequest ParentRequest { get; set; }
        public DateTime DateToBePaid { get; set; }
    }
}
