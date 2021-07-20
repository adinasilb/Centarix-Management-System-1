using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public abstract class ShareBase
    {
        [Key]
        public int ShareID { get; set; }
        public string FromApplicationUserID { get; set; }
        [ForeignKey("FromApplicationUserID")]
        public ApplicationUser FromApplicationUser { get; set; }
        public string ToApplicationUserID { get; set; }
        [ForeignKey("ToApplicationUserID")]
        public ApplicationUser ToApplicationUser { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TimeStamp { get; set; }
    }
}
