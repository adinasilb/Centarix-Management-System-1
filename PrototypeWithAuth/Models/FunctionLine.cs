﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FunctionLine
    {
        public int FunctionLineID { get; set; }
        public int LineID { get; set; }
        public Line Line { get; set; }
        public int FunctionTypeID { get; set; }
        public FunctionType FunctionType { get; set; }
        public TimeSpan Timer { get; set; }
        public string CommentText {get; set;}

    }
}
