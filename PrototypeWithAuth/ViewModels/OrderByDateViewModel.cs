using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using System.Globalization;


namespace PrototypeWithAuth.ViewModels
{
    public class OrderByDateViewModel
    {
        IEnumerable<ParentRequest> ParentRequests { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.Month);
            }
        }
        public int Total { get; set; }
    }
}
