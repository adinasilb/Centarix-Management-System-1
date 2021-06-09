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
        public int Installments { get; set; }
        public DateTime InstallmentDate { get; set; }
        public DateTime InstallmentDate_submit { get { return InstallmentDate; } set { InstallmentDate = value; } }
        public AppUtility.MenuItems SectionType { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
        public string RedirectAction { get; set; }
    }
}
