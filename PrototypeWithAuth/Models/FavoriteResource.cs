using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FavoriteResource :FavoriteBase
    {
        public int ResourceID { get; set; }
        public Resource Resource { get; set; }
    }
}
