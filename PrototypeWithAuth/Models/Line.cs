﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    [Table("Lines")]
    public class Line : LineBase
    {
        public  Line ParentLine { get; set; }
        public TempLine TempLine { get; set; }
    }
}
