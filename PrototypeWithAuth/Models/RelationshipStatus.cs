﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RelationshipStatus : ModelBase
    {
        [Key]
        public int RelationshipStatusID { get; set; }
        public string RelationshipStatusDescription { get; set; }
    }
}
