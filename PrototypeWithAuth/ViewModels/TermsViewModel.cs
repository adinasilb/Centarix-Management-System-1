using System;
using System.Collections.Generic;
using PrototypeWithAuth.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class TermsViewModel : ViewModelBase
    {
        public ParentRequest ParentRequest { get; set; }
        public int SelectedTerm { get; set; }
        public List<SelectListItem> TermsList { get; set; }
        //public int Taxes { get; set; } //does this go here?
        //public int Shipping { get; set; }
        //public int Credit { get; set; }
        public int Installments { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
    }
}
