using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Link
    {
        [Key]
        public int LinkID { get; set; }
        public string LinkDescription { get; set; }
        public string Url { get; set; }
        public int ProtocolID { get; set; }
        public Protocol Protocol { get; set;}
    }
}
