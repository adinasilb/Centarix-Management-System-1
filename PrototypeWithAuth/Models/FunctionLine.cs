using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FunctionLine: FunctionBase
    {
        public int LineID { get; set; }
        public Line Line { get; set; }

       // public bool IsTemporary { get; set; }

    }
}
