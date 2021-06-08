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
        [Key]
        public int FunctionLineID { get; set; }
        public int LineID { get; set; }
        public Line Line { get; set; }
        public int FunctionTypeID { get; set; }
        public FunctionType FunctionType { get; set; }
        public TimeSpan Timer { get; set; }
        public string Text {get; set;}
        public int? ProtocolID { get; set; }
        public Protocol Protocol { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        public bool IsTemporary { get; set; }
    }
}
