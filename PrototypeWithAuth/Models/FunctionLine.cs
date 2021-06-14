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
        [ForeignKey("TextID")]
        public Line Line { get; set; }
        public TimeSpan Timer { get; set; }
        public string Description {get; set;}
        
    }
}
