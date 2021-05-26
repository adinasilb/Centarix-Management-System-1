using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    [Table("TempLines")]
    public class TempLine : LineBase
    {
        //public TempLine ParentLine { get; set; }

        public int? PermanentLineID{ get; set; }
        public virtual Line PermanentLine { get; set; }
        [ForeignKey("PermanentLineID")]
        public TempLine ParentLine { get; set; }
    }
}
