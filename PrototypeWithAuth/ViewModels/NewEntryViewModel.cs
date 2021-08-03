using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class NewEntryViewModel
    {
        public DateTime DateTime { get; set; }
        public int VisitNumber { get; set; }
        public int SiteID { get; set; }
        public IEnumerable<Site> Sites { get; set; }
    }
}
