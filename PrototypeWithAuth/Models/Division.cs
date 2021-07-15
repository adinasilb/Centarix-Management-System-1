using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Division
    {
        [Key]
        public int DivisionID { get; set; }
        public string Description { get; set; }
        public IEnumerable<TestCategory> TestCategories { get; set; }
    }
}
