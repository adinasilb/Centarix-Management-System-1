using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public abstract class LineBase
    {
        [Key]
        public int LineID { get; set; }
        public string Content { get; set; }
        public int LineTypeID { get; set; }
        public LineType LineType { get; set; }
        public int ParentLineID { get; set; }
        public int ProtocolID { get; set; }
        public Protocol Protocol { get; set; }
        public int LineNumber { get; set; }
        public TimeSpan Timer { get; set; }
 
    }
}
