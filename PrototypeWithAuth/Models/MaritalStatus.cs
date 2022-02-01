﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class MaritalStatus : ModelBase
    {
        [Key]
        public int MaritalStatusID { get; set; }
        public string Description { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
