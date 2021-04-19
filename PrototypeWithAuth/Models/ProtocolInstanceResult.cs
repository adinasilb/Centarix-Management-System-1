using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class ProtocolInstanceResult
    {
        [Key]
        public int ResultID { get; set; }
        public string ResultDesciption {get; set;}
        public int ProtocolInstanceID { get; set; }
        public ProtocolInstance ProtocolInstance { get; set; }
    }
}
