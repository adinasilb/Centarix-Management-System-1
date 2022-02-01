﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ProtocolCategory : ModelBase
    {
        [Key]
        public int ProtocolCategoryTypeID { get; set; }
        public string ProtocolCategoryDescription { get; set; }
    }
}
