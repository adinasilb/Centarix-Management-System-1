using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CompanyDayOffType
    {
        [Key]
        public int CompanyDayOffTypeID { get; set; }
        public String Name { get; set; }

    }
}
