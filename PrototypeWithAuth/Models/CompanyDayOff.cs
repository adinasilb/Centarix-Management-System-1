using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CompanyDayOff
    {
        [Key]
        public int CompanyDayOffID { get; set; }
        public DateTime Date { get; set; }
        public int CompanyDayOffTypeID { get; set; }
        public CompanyDayOffType CompanyDayOffType { get; set; }
    }
}
