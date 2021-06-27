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
        public int TempRequestJsonID { get; set; }
        public Guid GuidID { get; set; }
        public String ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public String RequestJson { get; set; }
        public bool IsOriginal { get; set; }
        public bool IsCurrent { get; set; }

    }
}
