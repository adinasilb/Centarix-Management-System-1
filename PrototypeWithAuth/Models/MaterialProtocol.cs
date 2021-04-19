using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class MaterialProtocol
    {
        [Key, Column(Order = 1)]
        public int ProtocolID { get; set; }
        public Protocol Protocol { get; set; }
        [Key, Column(Order = 2)]
        public int MaterialID { get; set; }
        public Material Material { get; set; }
    }
}
