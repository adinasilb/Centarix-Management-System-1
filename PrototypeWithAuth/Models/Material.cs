﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Material : ModelBase
    {
        [Key]
        public int MaterialID { get; set; }
        public string Info { get; set; }
        public string Name { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        public int MaterialCategoryID { get; set; }
        public MaterialCategory MaterialCategory { get; set; }
        public ProtocolVersion ProtocolVersion { get; set; }
        public int ProtocolVersionID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
