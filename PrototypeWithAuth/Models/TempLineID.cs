using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TempLineID
    {
        [Key]
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
