using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FavoriteRequest
    {
        [Key]
        public int FavoriteRequestID { get; set; }
        public int RequestID { get; set; }
        [ForeignKey("RequestID")]
        public Request Request { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
