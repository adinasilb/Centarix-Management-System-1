using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class NewEntryViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        public DateTime Date_submit { get { return Date; } set { Date = value; } }
        public int VisitNumber { get; set; }
        public int SiteID { get; set; }
        public IEnumerable<Site> Sites { get; set; }
        public int ParticipantID { get; set; }
        public List<SelectListItem> VisitNumbers { get; set; }
    }
}
