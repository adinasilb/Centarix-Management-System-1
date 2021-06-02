using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class ProtocolType
    {
        [Key]
        public int ProtocolTypeID { get; set; }
        public string ProtocolTypeDescription { get; set; }
    }
}
