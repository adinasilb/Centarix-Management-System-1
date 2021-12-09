﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RequestLocationInstance : ModelBase
    {
        public int RequestID { get; set; }
        public Request Request { get; set; }
        public int LocationInstanceID { get; set; }
        public LocationInstance LocationInstance { get; set; }
        public int? ParentLocationInstanceID { get; set; }
        public LocationInstance ParentLocationInstance { get; set; }
        public bool IsArchived { get; set; }
    }
}
