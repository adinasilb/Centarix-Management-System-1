﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class ResourceType : ModelBase
    {
        [Key]
        public int ResourceTypeId { get; set; }
        public string ResourceTypeDescription { get; set; }
    }
}
