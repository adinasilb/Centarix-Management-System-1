using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class MaritalStatus
    {
        [Key]
        public int MaritalStatusID { get; set; }
        public string Description { get; set; }
    }
}
