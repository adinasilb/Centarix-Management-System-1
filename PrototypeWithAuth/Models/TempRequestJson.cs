using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TempRequestJson
    {
        [Key]
        public String CookieGUID { get; set; }
        public String ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public String RequestJson { get; set; }


    }
}
