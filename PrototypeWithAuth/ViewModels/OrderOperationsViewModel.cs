using System;
using System.Collections.Generic;
using PrototypeWithAuth.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class OrderOperationsViewModel
    {
        public int SelectedTerm { get; set; }
        public List<SelectListItem> TermsList { get; set; }
        public TempRequestListViewModel TempRequestListViewModel { get; set; }
        public StringWithBool Error { get; set; }
        public ParentRequest ParentRequest { get; set; }

    }
}
