﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class StringWithBool
    {
        public bool Bool { get; set; }
        public string String { get; set; }

        public void SetStringAndBool(bool Bool, string String)
        {
            this.Bool = Bool;
            this.String = String;
        }
    }
}
