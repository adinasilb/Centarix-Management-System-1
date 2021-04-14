using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ShareRequest
    {
        [Key]
        public int ShareRequestID { get; set; }
        public int RequestID { get; set; }
        public Request Request { get; set; }
        public string FromApplicationUserID { get; set; }
        [ForeignKey("FromApplicationUserID")]
        public ApplicationUser FromApplicationUser { get; set; }
        public string ToApplicationUserID { get; set; }
        [ForeignKey("ToApplicationUserID")]
        public ApplicationUser ToApplicationUser { get; set; }
    }
}
