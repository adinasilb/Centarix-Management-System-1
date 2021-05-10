using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FavoriteResource
    {
        [Key]
        public int FavoriteResourceID { get; set; }
        public int ResourceID { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
