﻿using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ShareRequest : ShareBase
    {

        [ForeignKey("ObjectID")]
        public Request Request { get; set; }
    }
}
