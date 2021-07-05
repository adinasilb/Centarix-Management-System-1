using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FavoriteProtocol:FavoriteBase
    {
        public int ProtocolID { get; set; }
        public Protocol Protocol { get; set; }
        
    }
}
