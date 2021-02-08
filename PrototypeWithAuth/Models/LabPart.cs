using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class LabPart
    {
        [Key]
        public int LabPartID { get; set; }
        public string LabPartName { get; set; }
    }
}
