using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Degree
    {
        [Key]
        public int DegreeID { get; set; }
        public string Description { get; set; }
    }
}
