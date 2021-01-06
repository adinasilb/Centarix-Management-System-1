using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class PartialOffDayType
    {
        [Key]
        public int PartialOffDayTypeID { get; set; }
        public string Description { get; set; }
        public IEnumerable<EmployeeHours> EmployeeHours { get; set; }
    }
}
