using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class PhysicalAddress
    {
        public int PhysicalAddressID { get; set; }
        public string EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
