﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    [Table("TempLines")]
    public class TempLine : LineBase
    {
        //public TempLine ParentLine { get; set; }
        public int? PermanentLineID { get; set; }
        [ForeignKey("PermanentLineID")]
        public Line PermanentLine { get; set; }
        //[ForeignKey("PermanentLineID")]
        public virtual TempLine ParentLine { get; set; }
        public virtual List<TempLine> TempLines { get; set; }
    }
}
