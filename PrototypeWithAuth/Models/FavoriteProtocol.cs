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
        public int ProtocolVersionID { get; set; }
        public ProtocolVersion ProtocolVersion { get; set; }
        
    }
}
