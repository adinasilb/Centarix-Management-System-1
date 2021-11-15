using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Sweater
    {
        [Key]
        public int SweaterID { get; set; }
        public string Description { get; set; }
    }
}
