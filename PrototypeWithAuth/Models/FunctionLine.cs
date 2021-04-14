using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FunctionLine
    {
        [Key, Column(Order = 1)]
        public int LineID { get; set; }
        public Line Line { get; set; }
        [Key, Column(Order = 1)]
        public int FunctionTypeID { get; set; }
        public FunctionType FunctionType { get; set; }
    }
}
