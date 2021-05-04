using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class AuthorProtocol
    {
        [Key, Column(Order = 1)]
        public int ProtocolID { get; set; }
        public Protocol Protocol { get; set; }
        [Key, Column(Order = 2)]
        public int AuthorID { get; set; }
        public Author Author { get; set; }
    }
}
